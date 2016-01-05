In this project we try to learn the motion of spheres in 3-space from depth maps generated in Unity.  We try to "shoot" these moving spheres with a bullet with fixed speed.  Our training data is the difference of two consecutive depth maps.  Because these depth maps were generated in Unity, we know the actual locations in 3-space and can therefore compute the true velocity.  From this velocity and the fixed speed of the bullet, we calculate the unique target point at which the bullet would hit the center of the sphere if it were launched perfectly.  We train the regression model on this target point.  Our test data is assessed, not on the difference between the true and predicted target point, but rather on whether we hit the test sphere at all given the predicted target.

Comments:

1.  Training is on the collision point in 3-space.  It might be better to train on direction (normalized vector).

2.  Overfitting seems unlikely but could be a problem and should be checked.  There are approximately 5000 depth maps and therefore approximately 2.5e7 velocities.  Each loop trains on 1000 random samples and tests on 1000 random samples.  So with several thousand loops there will be some overlap between training and validation sets.

3.  The image below shows training progress as our loss on the validation set is the percentage of spheres hit.
![Test Image]
(https://github.com/mettinger/machineLearningInvestigations/blob/master/depthMap/sphereProgress.png)