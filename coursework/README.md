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
Having suffered depression and anxiety, I decided to focus on these illnesses. In order to prevent myself considering my experiences as general, I asked many friends, who had dealt/are dealing with one or both, what their experiences were/are like. Many words were consistently repeated across my observations. 
\
\
Depression: isolated, hopeless, unsatisfied, cold, tired, despair, endless, unrelenting
\
Anxiety: jarring, overwhelmed, uncontrollable, worry, hyper-aware, fight-or-flight
\
\
Following this qualitative analysis, I selected the ocean scene to represent depression. I often considered battling depression was similar to traversing a stormy sea. An ocean seems *endless* and the idea of the player battling turbulent waves could convey *hopelessness* and *despair*, whilst trying to reach an *ioslated* lighthouse with seemingly few amnetities, evoking a degree of *dissatisfaction*. For anxiety, I chose the moon scene. It was particularly difficult for me to devise a natural environment to represent anxiety (possibly because many of the discussions attributed anxiety to particularly artificial means, such as social media). As such, the moon seemed the most ideal to add these man-made pressures and distractions, through the perspective of science fiction. Having the player glide *uncontrollably* with each step in low gravity as their heads up display *overwhelms* them with information, pointing towards the base that contains the necessary oxygen which is *worryingly* running out as they hear the *jarring* sounds of alarm.
#### Ocean Scene
For the ocean I forked ["#"](this) excellent project. It uses compute shaders and Fast Fourier transforms to create a computationally inexpensive realistic body of water. 
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
### Evaluation
### Future work
<!--- 
Quantitive evaluation as opposed to qualitative
Water width and height
--->