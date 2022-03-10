#include <Wire.h>
#include <TimerOne.h>
#include <MadgwickAHRS.h>

#define    MPU9250_ADDRESS            0x68
#define    MAG_ADDRESS                0x0C

#define    GYRO_FULL_SCALE_250_DPS    0x00  
#define    GYRO_FULL_SCALE_500_DPS    0x08
#define    GYRO_FULL_SCALE_1000_DPS   0x10
#define    GYRO_FULL_SCALE_2000_DPS   0x18

#define    ACC_FULL_SCALE_2_G        0x00  
#define    ACC_FULL_SCALE_4_G        0x08
#define    ACC_FULL_SCALE_8_G        0x10
#define    ACC_FULL_SCALE_16_G       0x18

// Mag calibration values are calculated via ahrs_calibration.
// These values must be determined for each baord/environment.
// See the image in this sketch folder for the values used
// below.

// Offsets applied to raw x/y/z values
float mag_offsets[3]            = { -2.20F, -5.53F, -26.34F };

// Soft iron error compensation matrix
float mag_softiron_matrix[3][3] = { { 0.934, 0.005, 0.013 },
                                    { 0.005, 0.948, 0.012 },
                                    { 0.013, 0.012, 1.129 } }; 

float mag_field_strength        = 48.41F;



// This function read Nbytes bytes from I2C device at address Address. 
// Put read bytes starting at register Register in the Data array. 
void I2Cread(uint8_t Address, uint8_t Register, uint8_t Nbytes, uint8_t* Data)
{
  // Set register address
  Wire.beginTransmission(Address);
  Wire.write(Register);
  Wire.endTransmission();
  
  // Read Nbytes
  Wire.requestFrom(Address, Nbytes); 
  uint8_t index=0;
  while (Wire.available())
    Data[index++]=Wire.read();
}


// Write a byte (Data) in device (Address) at register (Register)
void I2CwriteByte(uint8_t Address, uint8_t Register, uint8_t Data)
{
  // Set register address
  Wire.beginTransmission(Address);
  Wire.write(Register);
  Wire.write(Data);
  Wire.endTransmission();
}



// Initial time
long int ti;
volatile bool intFlag=false;
Madgwick filter;
// Initializations
void setup()
{
  // Arduino initializations
  Wire.begin();
  Serial.begin(115200);
  
  // Set accelerometers low pass filter at 5Hz
  I2CwriteByte(MPU9250_ADDRESS,29,0x06);
  // Set gyroscope low pass filter at 5Hz
  I2CwriteByte(MPU9250_ADDRESS,26,0x06);
 
  
  // Configure gyroscope range
  I2CwriteByte(MPU9250_ADDRESS,27,GYRO_FULL_SCALE_1000_DPS);
  // Configure accelerometers range
  I2CwriteByte(MPU9250_ADDRESS,28,ACC_FULL_SCALE_4_G);
  // Set by pass mode for the magnetometers
  I2CwriteByte(MPU9250_ADDRESS,0x37,0x02);
  
  // Request continuous magnetometer measurements in 16 bits
  I2CwriteByte(MAG_ADDRESS,0x0A,0x16);
  filter.begin(25);
   pinMode(13, OUTPUT);
  Timer1.initialize(10000);         // initialize timer1, and set a 1/2 second period
  Timer1.attachInterrupt(callback);  // attaches callback() as a timer overflow interrupt
  
  
  // Store initial time
  ti=millis();
}





// Counter
long int cpt=0;

void callback()
{ 
  intFlag=true;
  digitalWrite(13, digitalRead(13) ^ 1);
}

// Main loop, read and display data
void loop()
{
  while (!intFlag);
  intFlag=false;
  

  
  // _______________
  // ::: Counter :::
  
  // Display data counter
//  Serial.print (cpt++,DEC);
//  Serial.print ("\t");
  
 
 
  // ____________________________________
  // :::  accelerometer and gyroscope ::: 

  // Read accelerometer and gyroscope
  uint8_t Buf[14];
  I2Cread(MPU9250_ADDRESS,0x3B,14,Buf);
  
  // Create 16 bits values from 8 bits data
  
  // Accelerometer
  int16_t aix=-(Buf[0]<<8 | Buf[1]);
  int16_t aiy=-(Buf[2]<<8 | Buf[3]);
  int16_t aiz= Buf[4]<<8 | Buf[5];

  // Gyroscope
  int16_t gix=-(Buf[8]<<8 | Buf[9]);
  int16_t giy=-(Buf[10]<<8 | Buf[11]);
  int16_t giz=Buf[12]<<8 | Buf[13];
  
    // Display values
  
  // Accelerometer
 // Serial.print (aix,DEC); 
//  Serial.print ("\t");
 // Serial.print (aiy,DEC);
 // Serial.print ("\t");
 // Serial.print (aiz,DEC);  
 // Serial.print ("\t");
  
  // Gyroscope
 // Serial.print (gix,DEC); 
 // Serial.print ("\t");
 // Serial.print (giy,DEC);
 // Serial.print ("\t");
//  Serial.print (giz,DEC);  
 // Serial.print ("\t");

  
  // _____________________
  // :::  Magnetometer ::: 

  
  // Read register Status 1 and wait for the DRDY: Data Ready
  
  uint8_t ST1;
  do
  {
    I2Cread(MAG_ADDRESS,0x02,1,&ST1);
  }
  while (!(ST1&0x01));

  // Read magnetometer data  
  uint8_t Mag[7];  
  I2Cread(MAG_ADDRESS,0x03,7,Mag);
  

  // Create 16 bits values from 8 bits data
  
  // Magnetometer
  int16_t mix=-(Mag[3]<<8 | Mag[2]);
  int16_t miy=-(Mag[1]<<8 | Mag[0]);
  int16_t miz=-(Mag[5]<<8 | Mag[4]);
  


// Apply mag offset compensation (base values in uTesla)
  float x = mix - mag_offsets[0];
  float y = miy - mag_offsets[1];
  float z = miz - mag_offsets[2];


  float mx = x * mag_softiron_matrix[0][0] + y * mag_softiron_matrix[0][1] + z * mag_softiron_matrix[0][2];
  float my = x * mag_softiron_matrix[1][0] + y * mag_softiron_matrix[1][1] + z * mag_softiron_matrix[1][2];
  float mz = x * mag_softiron_matrix[2][0] + y * mag_softiron_matrix[2][1] + z * mag_softiron_matrix[2][2];
  
  // Magnetometer
  //Serial.print (mix+200,DEC); 
 // Serial.print ("\t");
 // Serial.print (miy-70,DEC);
 // Serial.print ("\t");
//  Serial.print (miz-700,DEC);  
//Serial.print ("\t");
  
  
  
  // End of line
 // Serial.println("");
//  delay(100);    

 float ax, ay, az;
  float gx, gy, gz;
  float roll, pitch, heading;
  
    ax = convertRawAcceleration(aix);
    ay = convertRawAcceleration(aiy);
    az = convertRawAcceleration(aiz);
    gx = convertRawGyro(gix);
    gy = convertRawGyro(giy);
    gz = convertRawGyro(giz);
    filter.update(gx, gy, gz, ax, ay, az, mx, my, mz);

    roll = filter.getRoll();
    pitch = filter.getPitch();
    heading = filter.getYaw();


  Serial.print (roll,DEC); 
  Serial.print ("*");
  Serial.print (pitch,DEC);
  Serial.print ("*");
  Serial.print (heading,DEC);  
  Serial.print ("*");
  Serial.println("");
  delay(1);  
}


float convertRawAcceleration(int aRaw) {
  // since we are using 2G range
  // -2g maps to a raw value of -32768
  // +2g maps to a raw value of 32767
  
  float a = (aRaw * 2.0) / 32768.0;
  return a;
}

float convertRawGyro(int gRaw) {
  // since we are using 250 degrees/seconds range
  // -250 maps to a raw value of -32768
  // +250 maps to a raw value of 32767
  
  float g = (gRaw * 250.0) / 32768.0;
  return g;
}