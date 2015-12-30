This project uses virtual reality (GearVR) to visualize the results of four dimensionality reduction techniques (PCA, t-SNE, Isomap, MDS) applied to MNIST data.

Try it out by installing gearVR.apk on a Samsung S6 and popping the phone into GearVR.  A gamepad is required.  Button A recenters the view to the start position.  Button B switches between the four methods.

The file mnistDimReduction.py in the python directory generates the data which is then packaged in the .apk application.  The file OVRPlayerController.cs in the cSharp direction is a small modification of the player controller script which reads in the data and displays it.