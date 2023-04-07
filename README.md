# Accurate Bird's Eye View
This project aimed to enhance localization accuracy in the field of soccer robots by combining data from the Inertial Measurement Unit (IMU) and camera frame. To achieve this, a simple implementation of the Madgwick algorithm was used with an Arduino Uno board and the MPU9250 sensor module, which served as the IMU. The method was successfully tested using the Monte Carlo particle filter localization algorithm.
By implementing this approach, the robot's distance from field lines and landmarks can be measured with greater precision. As the robot moves, the IMU calculates its deviation in the Roll, Pitch, and Yaw axes from the zero position at any moment. The rotational matrices are adjusted based on the errors computed by the IMU in all axes while calculating the bird's eye frame. This results in a more accurate localization algorithm by reducing measurement errors and ultimately improving positioning accuracy.
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

![15new](https://user-images.githubusercontent.com/6237268/228892566-198dc28b-c044-4005-9f43-f2edc42311a7.png)

The number of chessboards cells in the vertical axis is 𝑑𝑦 and the number of chessboards cells on the horizontal axis is 𝑑𝑥 when we have &nbsp;&nbsp;&nbsp; 𝑟𝑥 = 𝑑𝑥/𝑃𝑥  &nbsp;&nbsp;&nbsp; and &nbsp;&nbsp;&nbsp; 𝑟𝑦 = 𝑑𝑦/𝑃𝑦 distance and angle between robot and landmark can be calculated by: </br>
𝑅 = √(𝑑 + 𝑟𝑦 × ℎ)2 + (𝑟𝑥 × 𝑤)2  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   ∠𝐴 = tan−1((𝑟𝑥 × 𝑤)/(𝐷 + 𝑟𝑦 × ℎ))

![7e](https://user-images.githubusercontent.com/6237268/157687748-6125ba57-44d8-44b6-8066-ed72d4203f03.PNG)
![result](https://user-images.githubusercontent.com/6237268/157692804-dea4797a-d3da-4adf-9de9-537be91ac61a.png)

