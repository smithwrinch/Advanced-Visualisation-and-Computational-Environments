# Advanced Visualisation and Environments Project
This is my repository for my final project in the advanced visualisation and environments project.
### Motivation
From my project pitch I suggested two possible projects. The first actually came to me in a dream and featured four continuous scenes. Crossing the ocean in a boat to a lighthouse, jumping on the moon to a base, roaming through the desert into a temple, before finally travelling through a forest into a cabin. The idea centered around going through each of these scenes three times before finishing, each time with a subtle difference. I was drawn to the concept as I wanted to work on crafting realistic virtual environments as well as improving my limited modelling knowledge in Blender. 
\
(cabin in the woods vibe)
\
\
\
The second suggestion was an attempt for people to be educated and/or understand mental health through interactive means. I wanted to explore the idea of evoking similar emotions and experiences felt by a person suffering from mental health problems. I considered a game might be perfect for this, not only because it is engaging, but because of the powerful immersion games can afford. With the right environment, you can be placed into the protagonists shoes and see completely from their perspective. This could prove useful, since - having myself and close friends deal with mental health issues in the past - I know it can be frustrating for friends and family to try and empathise and understand what the sufferer is going through, having not experienced these issues themselves. The game could be used as a tool to bridge this gap and hopefully provide help on both ends. With this forming the proposal, I gave little thought as to how it might be accomplished (the hard part!). I initially aimed for as much abstraction and simplicity as possible: having just the player be a cube going through simple tasks. This would be an attempt to isolate the mental health aspect, for instance, having the cube reach a goal that progressively gets harder to reach before an easy task becomes impossible, such as with depression. Perhaps the colours could gradually become overwhelming to convey the experience of anxiety.
\
\
After getting feedback, my supervisor suggested combining the two ideas. I knew this would be a difficult task, since the mental health aspect had to be perfect and I couldn't hide behind abstraction, but instead create realistic scenes. It was paramount to me not to disrespect or trivialise my own or anyone elses mental health experiences. Consequently, I ensured sufficient research was carried out and applied with clinical precision. 
\
\
Adding to this, it is important to state that that everyone experiences mental health differently. This project can in no way encapsulate the entire range of emotions and feelings mental health causes.
### Inspiration
- Bioshock infinite
- Depression game
- Kerbal Space Program
- Limbo

### Process
Having suffered depression and anxiety, I decided to focus on these illnesses. In order to prevent myself considering my experiences as general, I asked many friends, who had dealt/are dealing with one or both, what their experiences were/are like. These were some of the most common words repeated across my observations. 
\
\
Depression: isolated, hopeless, unsatisfied, cold, tired, despair, endless, unrelenting, unfulfilment,
\
Anxiety: jarring, overwhelmed, uncontrollable, pressured, worry, hyper-aware, fight-or-flight, alienated
\
\
Following this qualitative analysis, I selected the ocean scene to represent depression. I often considered battling depression was similar to traversing a stormy sea. An ocean seems *endless* and the idea of the player battling turbulent waves could convey *hopelessness* and *despair*, whilst trying to reach an *ioslated* lighthouse with seemingly few amnetities, evoking a degree of *dissatisfaction*. For anxiety, I chose the moon scene. It was particularly difficult for me to devise a natural environment to represent anxiety (possibly because many of the discussions attributed anxiety to particularly artificial means, such as social media). As such, the moon seemed the most ideal to add these man-made pressures and distractions, through the perspective of science fiction. Having the player glide *uncontrollably* with each step in low gravity as their heads up display *overwhelms* them with information, pointing towards the base that contains the necessary oxygen which is *worryingly* running out as they hear the *jarring* sounds of alarm.
#### Ocean Scene
For the ocean I forked [this](#) excellent project. It uses compute shaders and Fast Fourier transforms to create a computationally inexpensive realistic body of water. 
<!--- PICTURE OF OCEAN --->
I will briefly explain how this works. Realistic wave patterns can be formed using Gerber waves. Gerber waves are the waveform produced when each "node" oscillates about a circle. The waves feature increased authenticity when Gerber waves with different parameters are summated. However, even if this were to be done through shaders, the computation quickly becomes expensive with large amounts of vertices - with a complexity of O(n^2). Using discrete FFTs to summate these waves drastically increases efficiency, with a complexity of O(nlogn). The vertices are calculated through compute shaders in this fashion, they are also rendered in cascades such that the quality/computation diminishes as you go further from the player camera.
<!--- Photo of Gerber Wave --->
I then needed the boat to be displaced according to the ocean vertices it rested on. Whilst I modelled everything else, I did not have time to model a boat so I used a free prefab from Unity's asset store. Additionally, the boats aesthetic was exactly how I imagined, and any attempt I made to replicate would be a poor mans copy. After spending considerable time understanding how the ocean worked, it was pretty easy to reverse engineer and extrapolate vertex heights at a given position. For realistic buoyancy, I created a script called Floater:
```
rigidBody.AddForceAtPosition(Physics.gravity/floaterCount, transform.position, ForceMode.Acceleration);
if(transform.position.y < waveHeight)
{
    float displacementMultplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
    rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultplier, 0f), transform.position, ForceMode.Acceleration);
    rigidBody.AddForce(displacementMultplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
    rigidBody.AddTorque(displacementMultplier * -rigidBody.angularVelocity* waterAngularDrag* Time.fixedDeltaTime, ForceMode.VelocityChange);
}
```
This behaviour was tied to four empty game objects (floaters) under the boat object. These directly translated the effect of the water at each corner of the boat. I then added a controller which applied forces to the boat subject to WASD keys. There would also be logic to reset the boat to its initial position if it capsised. Below is a snippet of the code used to illustrate the realistic motion of the paddles (it took me far too many headaches to get this right):
```
private void rotateRightPaddle(float factor){
    t_r -= (Mathf.Abs(factor)/ factor)*Time.deltaTime;
    float x = Mathf.Abs(Mathf.Sin(t_r));
    float y = Mathf.Cos(t_r);
    float z = -Mathf.Sin(t_r);
    paddleRight.transform.localRotation = Quaternion.Euler(new Vector3(x*50, y*50 - 180, z*30));
}
```
The boat mechanics were almost complete. However, the water would render inside the boat which made the vessel seem like it was sinking. To fix this I added a depth mask to a clone copy of the boat, but upside down. This drew onto the depth buffer.
```
Shader "Masked/Mask" {
	SubShader {
		Tags {"Queue" = "Geometry+10" }
		// Don't draw in the RGBA channels; just the depth buffer
		ColorMask 0
		ZWrite On
		Pass {}
	}
}
```
![upside_down](upside_down.PNG)
Now that I was pleased with the motion, I needed to add the lighthouse. Having rarely used Blender to model before, I followed [this]() tutorial using below as the reference picture:
\
![reference lighthouse](Capture.PNG)
\
After I was happy with the lighthouse, I modelled some rocks. I subdivided a cube and applied a displacement modifier following a Voronoi texture. I also created a custom material with a geometry node to change its pointiness. These rocks were planned to be around the lighthouse, but when I translated them to unity, they lost their low polygon count as the modifiers were rasterised. As a result, I used rocks from the unity asset store instead, to keep the game lag-free.  
\
![lighthouse](LIGHTHOUSE.PNG)
![lighthouse and rocks](modeling.PNG)
![rocks](shading.PNG)
\
I found it difficult to properly translate the materials I made in Blender, so I re-made them in Unity. A seamless concrete texture (with normal maps) and a standard black material were applied to the lighthouse. I then modelled the dock and door. 
\
![dock](doock.PNG)
![door](dooor.PNG)
\
I modelled the door such that the inner door could be rotated around the hinges. There were a few teething problems:
\
![door](dor.PNG)
\
After configuring the logic once the player gets to the dock, I added a spotlight with a simple volumetric shader and a spotlight source. I created a simple yellow gradient texture in photoshop and applied it to a modified cylinder made in blender. This rotated around the center of the lighthouse. Then I had to calibrate the wave settings for each subsequent level. Below are some pretty crazy waves generated from messing around in the settings:
\
![impossible](impossible.PNG)
![crazy](crazy.PNG)
#### Moon Scene
The main component in the moon scene was the realistic terrain generator. This would made using the summation of perlin noise waves with different degrees of detail (FM) and displacement (AM). I initially wanted to do this using a vertex shader, but I got into difficulty getting the vertex heights to the CPU. Whilst I could have fiddled around with a compute shader, being confined by time I decided to generate the mesh on the CPU - but in an efficient way. 
\
![moon](mun.PNG)
![moon](surface.PNG)
\
Programmatically I arranged an variable amount of vertices in a square grid. These could be scaled as well as altered in the settings. Each vertex y position was set to the height returned by the function below:
```
private float calculateHeight(float x, float z){
    x/=scale;
    z/=scale;        
    x += xSize * getXOffset();
    z += zSize * getZOffset();

    float low = Mathf.PerlinNoise(x*0.0051f, z*0.005f) * 60f * scale;
    float med = Mathf.PerlinNoise(x*0.1f, z*0.11f) * 3f * scale;
    float high = Mathf.PerlinNoise(0.3f*x, 0.31f*z) *scale;
    return low + med + high; 
}
 ```
To ensure the generation was efficient, I loaded the terrain in chunks. These were loaded in and out subject to where the player was standing, in such a way that the player was always in the middle chunk of a 5x5 grid. There were some issues lining these chunks up correctly:
\
![lining chunks](terrain.PNG)
\
By multiplying the size by the x,y offset of each chunk, the height variable was consistent. It took many attempts of trial and error to get the height formula to a standard I was happy with:
\
![oops height](oop.PNG)
\
The next task was to ensure that the textures between chunks lined up correctly. I decided to add a custom surface shader that allowed you to scale the UV coordinates (which were just taken to be the vertex position). Whilst it wasn't perfect, it was much less noticeable than before. I also added functionality for a bump map.
\
![moon shaded](shader.PNG)
![moon shaded 2](shaderr.PNG)
\
Now that terrain was sorted, I needed to add the ability to control the player. I added a mesh collider to the terrain and a rigid body to the player, with reduced gravity. Logic was placed such that the player could only move if they were touching the ground. Next, I modelled a moon base in blender. 
\
![moon base](basee.PNG)
\
### Walkthrough
There are currently three game modes to select from: both depression and anxiety, just depression, and just anxiety. These can be selected through the menu:
\
![menu scene](menu.PNG)
\
Clicking begin, you will play through both scenes alternatley whereas the other two plays through just one scene. There are three levels to each scene which, on completion, reverts back to the starting level. This was an artistic decision in an attempt to represent the fact that mental health conditions are ongoing - and can go through repeated cycles of intensity. Below is the scene shown when the player clicks "About":
\
![about scene](about.PNG)
#### Ocean Scene
##### Level 1
![about scene](ol1.PNG)
##### Level 3
![about scene](ol3.PNG)
##### The lighthouse
![about scene](lighthouse.PNG)
#### Moon Scene
##### Level 1
![about scene](ol1.PNG)
![about scene](ol3.PNG)
##### Level 3
##### The moonbase
![about scene](lighthouse.PNG)
\
If you would like to see a video walkthrough, please click [here]().
### Evaluation
Whilst I am pleased with what I have created, I would not consider this work complete. Most clearly, a lot more focus was placed on the ocean/depression scene and it's aesthetic, compared to the moon/anxiety scene. This was due to being too ambitious with what I could achieve - running out of time. There were many more snags along the way than I anticipated/allocated time for. This project remains a humbling reminder as to how head-achey game development can be! 
\
\
Thinking back to the motivations, there were two main purposes of this project. The first, and least significant, was to improve my knowledge of Blender. Going from near zero experience to modelling all but one model in the project, I feel that I satisfied this goal. The most important purpose for this work, however, was to give people accurate insights into the experience of those dealing with anxiety and depression. From my persepctive TODO
\
\
However, in order to have a thourough evaluation, I asked some of my friends (both sufferers and non-sufferers of mental health issues) to play the game. 
### Future work
As aforementioned in the evaluation, work remains to be done before I am completely happy with this project. To begin, I would like to carry out more research from people suffering from mental health conditions - ideally in a more quantative manner. This time round I could gather data in the form of feedback from the game in its current state, as well as a more tailored survey asking questions before and after seeing the game. 
\
\
For the ocean scene I would like to add weather to add some pathetic fallacy - hopefully culiminating in a more somber mood. It was rather low down on my priorities and I ran out of time. Also, it was important to me the scene for depression wasn't classically sad - as most people deem depression to only be. In my mind, and others from discussions, depression manifests itself differently to sadness - but it is still a symptom. In this way, I would like to experiment with the weather. Additionally, I would like to improve the lighthouses beam of light, I feel it's simplicity detracts from the realness of the scene. Other minor changes would include an affect on the (currently non-existant) HUD, as the water aggressively crashes with the boat. The boat movement may also be improved, by allowing the water to inflict a horizontal force component, as opposed to just the vertical.
\
\
The moon scene, in my opinion, needs more work than the lighthouse scene in terms of realism. Perhaps adding some fog at the edges of the grid, or a more realistic light source could fix the rather artificial terrain. Additionally, I would like to replace the skybox with one that is more dynamic, maybe also including a sun with a lens flare. The crater in which the moon base spawns also looks very artificial, I plan to work on tapering the edges to a flat bed as opposed to suddenly changing to flat.
\
\
Finally, I would love to accomodate for other mental health conditions/disorders, such as bipolar disorder, schizophrenia, ADHD, and autism. Depending on the feedback, I would go through a similar process as with anxiety and depression. Ideally, adding VR functionality would complete the project - I believe this would the perfect medium to deploy and emulate these perspectives and experiences. Of course at this stage, the project would have to be heavily adapted with the possibility of a complete overhaul in the gameplay mechanics.  
