# AccurateBirdEyeView
This project was implemented to improve localization accuracy in the soccer robot field by concatenating Inertial Measurement Unit (IMU) data and camera input. A simple implementation of the Madgwick algorithm on the Arduino Uno board, along with the MPU9250 sensor module used for the IMU, was successfully tested using the Monte Carlo particle filter localization algorithm.
With this method, the robot's distance from field lines and landmarks can be measured more accurately. As the robot moves, the IMU calculates its deviation in the Roll, Pitch, and Yaw axes from the zero position at any moment. While calculating the bird's eye frame, the inverse effect is applied to the rotational matrices to account for errors computed by the IMU in all axes. This approach reduces measurement errors and improves positioning accuracy as a result.
</br>
</br>
![2e](https://user-images.githubusercontent.com/6237268/157687335-33a461bd-7ca9-4091-a374-81613b90ca6f.PNG)
![3e](https://user-images.githubusercontent.com/6237268/157687172-1cc59c27-fbf9-4aee-87a3-caca8f6c7037.PNG)
![4e](https://user-images.githubusercontent.com/6237268/157688079-72adad11-4f4f-4e3e-96d3-cbb908dcf8f2.PNG)
![11e](https://user-images.githubusercontent.com/6237268/157687099-311c6785-90cc-476d-84c9-32cbeb2ee9c8.PNG)

To calculate the birds eye frame we have:
</br> (𝑢, 𝑣, 1)𝑇=KTR(𝑥, 𝑦, 𝑧, 1)𝑇    R is 𝑹𝒙 × 𝑹𝒚 × 𝑹𝒛  which forms the 3 rotation matrices

![1e](https://user-images.githubusercontent.com/6237268/157687484-85934d21-28a0-4caf-b2d2-7a0d875d2f4f.PNG)
![5e](https://user-images.githubusercontent.com/6237268/157687506-ff520a0f-bad6-453f-b52b-c53de6935016.PNG)

![33432se](https://user-images.githubusercontent.com/6237268/157710243-34d2d005-8980-4e6e-978b-67e2b1280274.png)

The number of chessboards cells in the vertical axis is 𝑑𝑦 and the number of chessboards cells on the horizontal axis is 𝑑𝑥 when we have &nbsp;&nbsp;&nbsp; 𝑟𝑥 = 𝑑𝑥/𝑃𝑥  &nbsp;&nbsp;&nbsp; and &nbsp;&nbsp;&nbsp; 𝑟𝑦 = 𝑑𝑦/𝑃𝑦 distance and angle between robot and landmark can be calculated by: </br>
𝑅 = √(𝑑 + 𝑟𝑦 × ℎ)2 + (𝑟𝑥 × 𝑤)2  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   ∠𝐴 = tan−1((𝑟𝑥 × 𝑤)/(𝐷 + 𝑟𝑦 × ℎ))

![7e](https://user-images.githubusercontent.com/6237268/157687748-6125ba57-44d8-44b6-8066-ed72d4203f03.PNG)
![result](https://user-images.githubusercontent.com/6237268/157692804-dea4797a-d3da-4adf-9de9-537be91ac61a.png)

Special thanks to Dr. Rainer Hessmer because of his [awesome project](http://www.hessmer.org/robotics/monte-carlo-location-for-robots.html/) that has been used in a part of this repository.
