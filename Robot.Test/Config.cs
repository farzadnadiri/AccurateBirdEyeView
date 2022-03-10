using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Test
{
    public static class Config
    {
        public static class Walk
        {
       
            //////////////////////////////////////////////
// Stance and velocity limit values
//////////////////////////////////////////////
       public     static List<double> stanceLimitX=new List<double> {-0.10,0.10};
       public     static List<double> stanceLimitY=new List<double> {0.07,0.20};
        public    static List<double> stanceLimitA=new List<double> {0*Math.PI/180,30*Math.PI/180};

        public    static List<double> velLimitX=new List<double> {-.03,.08};
        public    static List<double> velLimitY=new List<double> {-.03,.03};
         public   static List<double> velLimitA=new List<double> {-.4,.4};
            public static List<double> velDelta = new List<double> {0.02, 0.02, 0.15}; 

//////////////////////////////////////////////
// Stance parameters
////////////////////////////////////////////-
 public    static double bodyHeight = 0.295; 
 public    static double bodyTilt=20*Math.PI/180; 
 public    static double footX= -0.020; 
 public    static double footY = 0.035;
 public    static double supportX = 0;
 public    static double supportY = 0.010;


public    static List<double> qLArm=new List<double>{Math.PI/180*90,Math.PI/180*2,Math.PI/180*-40};
public    static List<double> qRArm=new List<double>{Math.PI/180*90,Math.PI/180*-2,Math.PI/180*-40};

public static List<double> qLArmKick=new List<double>{Math.PI/180*90,Math.PI/180*30,Math.PI/180*-60};
public static List<double> qRArmKick=new List<double>{Math.PI/180*90,Math.PI/180*-30,Math.PI/180*-60};

public static double hardnessSupport = 1;
public static double hardnessSwing = 1;
public static double hardnessArm=.3;
////////////////////////////////////////////-
// Gait parameters
////////////////////////////////////////////-
public static double tStep = 0.25;
public static double tZmp = 0.165;
public static double stepHeight = 0.035;
public  static List<double> phSingle=new List<double>{0.1,0.9};

////////////////////////////////////////////
// Compensation parameters
////////////////////////////////////////////
public static double hipRollCompensation = 4*Math.PI/180;
public static List<double>ankleMod = new List<double>{-1*Math.PI/180,0*Math.PI/180};
public static double spreadComp = 0.015;

//////////////////////////////////////////////////////////////
//Imu feedback parameters, alpha / gain / deadband / max
//////////////////////////////////////////////////////////////
public static double gyroFactor = 0.273*Math.PI/180 * 300 / 1024; //dps to rad/s conversion


 public static List<double> ankleImuParamX=new List<double>{0.5,0.3*gyroFactor,
                        1*Math.PI/180, 25*Math.PI/180};
 public static List<double> kneeImuParamX=new List<double>{0.5,1.2*gyroFactor,
                        1*Math.PI/180, 25*Math.PI/180};
 public static List<double> ankleImuParamY=new List<double>{0.5,0.7*gyroFactor,
                        1*Math.PI/180, 25*Math.PI/180};
 public static List<double> hipImuParamY=new List<double>{0.5,0.3*gyroFactor,
                        1*Math.PI/180, 25*Math.PI/180};
 public static List<double> armImuParamX=new List<double>{0.3,10*gyroFactor, 20*Math.PI/180, 45*Math.PI/180};




////////////////////////////////////////////
// Support point modulation values
////////////////////////////////////////////

public static double velFastForward = 0.05;
public static double velFastTurn = 0.15;

//List<double> supportFront = 0.01; //Lean back when walking fast forward
public static double supportFront = 0.03; //Lean back when walking fast forward

public static double supportFront2 = 0.03; //Lean front when accelerating forward
public static double supportBack = -0.02; //Lean back when walking backward



public static double supportSideX = -0.005; //Lean back when sidestepping

public static double supportSideY = 0.02; //Lean sideways when sidestepping


public static double supportTurn = 0.02; //Lean front when turning


public static double turnCompThreshold = 0.1;


public static double turnComp = 0.003; //Lean front when turning


////////////////////////////-
    
             
        }
    }
}
