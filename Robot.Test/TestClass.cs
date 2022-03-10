using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Test
{
   public class TestClass
    {


/* Walk Parameters
 Stance and velocity limit values
 */
        double stanceLimitX = 0;// {-0.10 , 0.10};
        double stanceLimitY = 0;//0;//Config.walk.stanceLimitY or {0.09 , 0.20};
        double stanceLimitA = 0;//0;//Config.walk.stanceLimitA or {-0*math.pi/180, 40*math.pi/180};
        private double velLimitX = 0;// 0;//Config.walk.velLimitX or {-.06, .08};
        private double velLimitY = 0;// 0;//Config.walk.velLimitY or {-.06, .06};
        private double velLimitA = 0;// 0;//Config.walk.velLimitA or {-.4, .4};
        double velDelta = 0;// 0;//Config.walk.velDelta or {.03,.015,.15};

//Toe/heel overlap checking values
        double footSizeX = 0;// 0;//Config.walk.footSizeX or {-0.05,0.05};
        double stanceLimitMarginY = 0;// 0;//Config.walk.stanceLimitMarginY or 0.015;
        double stanceLimitY2 = 0;// 2* 0;//Config.walk.footY-stanceLimitMarginY;

//OP default stance width: 0.0375*2 = 0.075
//Heel overlap At radian 0.15 at each foot = 0.05*sin(0.15)*2=0.015
//Heel overlap At radian 0.30 at each foot = 0.05*sin(0.15)*2=0.030

//Stance parameters
       static double bodyHeight = 0;// 0;//Config.walk.bodyHeight;
        private static double bodyTilt = 0;//0;//Config.walk.bodyTilt or 0;
        double footX = 0;// 0;//Config.walk.footX or 0;
        private static double footY = 0;// 0;//Config.walk.footY;
        private static double supportX = 0;// 0;//Config.walk.supportX;
        double supportY = 0;// 0;//Config.walk.supportY;
        private double qLArm0 = 0;//0;//Config.walk.qLArm;
        private double qRArm0 = 0;//0;//Config.walk.qRArm;
        private double qLArmKick0 = 0;//0;//Config.walk.qLArmKick;
        private double qRArmKick0 = 0;//0;//Config.walk.qRArmKick;

//Hardness parameters
double hardnessSupport = 0;//Config.walk.hardnessSupport or 0.7;
double hardnessSwing = 0;//Config.walk.hardnessSwing or 0.5;
double hardnessArm = 0;//Config.walk.hardnessArm or 0.2;

//Gait parameters
double tStep0 = 0;//Config.walk.tStep;
double tStep = 0;//Config.walk.tStep;
double tZmp = 0;//Config.walk.tZmp;
double stepHeight = 0;//Config.walk.stepHeight;
       private const double ph1Single = 0; //Config.walk.phSingle[1];
       private const double ph2Single = 0; //Config.walk.phSingle[2];
       double ph1Zmp=ph1Single;
private double ph2Zmp = ph2Single;

//Compensation parameters
double hipRollCompensation = 0;//Config.walk.hipRollCompensation;
double ankleMod = 0;//Config.walk.ankleMod or {0,0};

//Gyro stabilization parameters
double ankleImuParamX = 0;//Config.walk.ankleImuParamX;
double ankleImuParamY = 0;//Config.walk.ankleImuParamY;
double kneeImuParamX = 0;//Config.walk.kneeImuParamX;
double hipImuParamY = 0;//Config.walk.hipImuParamY;
double armImuParamX = 0;//Config.walk.armImuParamX;
double armImuParamY = 0;//Config.walk.armImuParamY;

//WalkKick parameters
double walkKickVel = 0;//Config.walk.walkKickVel or {.05, .08};
double walkKickSupportMod = 0;//Config.walk.walkKickSupportMod or {{-0.03,0},{-0.03,0}};
double walkKickHeightFactor = 0;//Config.walk.walkKickHeightFactor or 1.5;
double tStepWalkKick = 0;//Config.walk.tStepWalkKick or tStep;

//Sidekick parameters 
double sideKickVel1 = 0;//Config.walk.sideKickVel1 or {0.04,0.04};
double sideKickVel2 = 0;//Config.walk.sideKickVel2 or {0.09,0.05};
double sideKickVel3 = 0;//Config.walk.sideKickVel3 or {0.09,-0.02};
double sideKickSupportMod = 0;//Config.walk.sideKickSupportMod or {{0,0},{0,0}};
double tStepSideKick = 0;//Config.walk.tStepSideKick or 0.70;

//Support bias parameters to reduce backlash-based instability
double supportFront = 0;//Config.walk.supportFront or 0;
double supportBack = 0;//Config.walk.supportBack or 0;
double supportSideX = 0;//Config.walk.supportSideX or 0;
double supportSideY = 0;//Config.walk.supportSideY or 0;

//////////////////////////////////////////////////////////
// Walk state variables
//////////////////////////////////////////////////////////

List<double> uTorso = new List<double>{supportX, 0, 0};
List<double> uLeft = new List<double>{0, footY, 0};
List<double> uRight = new List<double>{0, -footY, 0};

List<double> pLLeg = new List<double>{0, footY, 0, 0,0,0};
List<double> pRLeg = new List<double>{0, -footY, 0, 0,0,0};
List<double> pTorso = new List<double>{supportX, 0, bodyHeight, 0,bodyTilt,0};

List<double> velCurrent = new List<double>{0, 0, 0};
List<double> velCommand = new List<double>{0, 0, 0};
List<double> velDiff = new List<double>{0, 0, 0};

//ZMP exponential coefficients:
double aXP, aXN, aYP, aYN = 0;

//Gyro stabilization variables
List<double> ankleShift = new List<double>{0, 0};
double kneeShift = 0;
List<double> hipShift = new List<double>{0,0};
List<double> armShift = new List<double>{0, 0};

bool Newactive = true;
bool Newstarted = false;
double iStep0 = -1;
double NewiStep = 0;
       double t0 = 0;//Body.get_time();
       private double tLastStep = 0;// Body.get_time();

double NewstopRequest = 2;
double canWalkKick = 1; //Can we do walkkick with this walk code?
double walkKickRequest = 0; 
double walkKickType = 0;

double initial_step=2;
//////////////////////////////////////////////////////////
// } initialization 
//////////////////////////////////////////////////////////
        List<double>  uLeft1;
 List<double> uLeft2 ;
 List<double>    uRight1 ; 
 List<double>  uRight2 ;
 List<double>  uTorso1;
 List<double>    uTorso2 ;
 List<double> uSupport ;
void entry(){
  Console.WriteLine ("Motion: Walk entry");
  //SJ: now we always assume that we start walking with feet together
  //Because joint readings are not always available with darwins
  uLeft = Util.pose_global(new List<double>({-supportX, footY, 0}),uTorso);
  uRight = Util.pose_global(new List<double>({-supportX, -footY, 0}),uTorso);

   uLeft1=uLeft;
  uLeft2 =uLeft;
     uRight1 = uRight; 
   uRight2 = uRight;
   uTorso1= uTorso;
     uTorso2 = uTorso;
  uSupport = uTorso;

  //Place arms in appropriate position at sides
  //Body.set_larm_command(qLArm0);
  //Body.set_larm_hardness(hardnessArm);
  //Body.set_rarm_command(qRArm0);
  //Body.set_rarm_hardness(hardnessArm);

  walkKickRequest = 0;
}

       private List<double> supportMod;
    
string update()
{
    var t = 0;//Body.get_time();

  //Don't run update if the robot is sitting or standing
    var bodyHeightCurrent = 0;// vcm.get_camera_bodyHeight();
    if (bodyHeightCurrent < bodyHeight - 0.01)
    {
        return "0";
    }

    if (!Newactive)
    {
        update_still();
    }


    if (!Newstarted)
    {
        Newstarted = true;

        tLastStep = 0; // Body.get_time();
    }
    //SJ: Variable tStep support for walkkick
  var ph = (t-tLastStep)/tStep;
    if (ph > 1)
    {
        NewiStep = NewiStep + 1;
        ph = ph - Math.Floor(ph);
        tLastStep = tLastStep + tStep;
    }

    //Stop when stopping sequence is done
    if (NewiStep > iStep0 && NewstopRequest == 2)
    {
        NewstopRequest = 0;
        Newactive = false;
        return "stop";
    }
    // New step
  if (NewiStep > iStep0) {
    update_velocity();
    iStep0 = NewiStep;
    var supportLeg = NewiStep % 2; // 0 for left support, 1 for right support
    var uLeft1 = uLeft2;
    var uRight1 = uRight2;
    var uTorso1 = uTorso2;

    supportMod = new List<double>{0,0}; //Support Point modulation for walkkick
   var shiftFactor = 0.5; //How much should we shift final Torso pose?

    if (walkKickRequest>0)
      {
          check_walkkick();
      }
      //If stop signal sent, put two feet together
    else if (NewstopRequest == 1)
    {
        //Final step
        NewstopRequest = 2;
        velCurrent = new List<double> {0, 0, 0};

        velCommand = new List<double> {0, 0, 0};

        if
            (supportLeg == 0)
        {
            // Left support
            uRight2 = Util.pose_global(
                new List<double>
                {
                    0,
                    -2*footY,
                    0
                }
                ,
                uLeft1)
                ;
        }
        else // Right support
        {
            uLeft2 = Util.pose_global(
                new List<double>
                {
                    0,
                    2*footY,
                    0
                }
                ,
                uRight1)
                ;
        }
    }
    else
    {
        tStep = tStep0;
        if (
            supportLeg == 0)
        {
            uRight2 = step_right_destination(velCurrent, uLeft1, uRight1);
        }
        else
        {
            uLeft2 = step_left_destination(velCurrent, uLeft1, uRight1);
        }

        //Velocity-based support point modulation
        if
            (velCurrent[1] > 0.06)
        {
            supportMod[1] = supportFront;
        }
        else if
            (velCurrent[1] < 0)
        {
            supportMod[1] = supportBack;
        }
        if
            (velCurrent[2] > 0.015)
        {
            supportMod[1] = supportSideX;
            supportMod[2] = supportSideY;
        }
        else if
            (velCurrent[2] < -0.015)
        {
            supportMod[1] = supportSideX;
            supportMod[2] = -supportSideY;
        }
    }
      uTorso2 = step_torso(uLeft2, uRight2,shiftFactor);

    //Apply velocity-based support point modulation for uSupport
    if (supportLeg == 0) { //LS
      var uLeftTorso = Util.pose_relative(uLeft1,uTorso1);
      var uTorsoModded = Util.pose_global(
      new List<double>({supportMod[1],supportMod[2],0}),uTorso);
      var uLeftModded = Util.pose_global (uLeftTorso,uTorsoModded); 
      uSupport = Util.pose_global(
      {supportX, supportY, 0},uLeftModded);
      Body.set_lleg_hardness(hardnessSupport);
      Body.set_rleg_hardness(hardnessSwing);
    else //RS
      var uRightTorso = Util.pose_relative(uRight1,uTorso1);
      var uTorsoModded = Util.pose_global(
      new List<double>({supportMod[1],supportMod[2],0}),uTorso);
      var uRightModded = Util.pose_global (uRightTorso,uTorsoModded); 
      uSupport = Util.pose_global(
      {supportX, -supportY, 0}, uRightModded);
      Body.set_lleg_hardness(hardnessSwing);
      Body.set_rleg_hardness(hardnessSupport);
    }

    //Compute ZMP coefficients
    m1X = (uSupport[1]-uTorso1[1])/(tStep*ph1Zmp);
    m2X = (uTorso2[1]-uSupport[1])/(tStep*(1-ph2Zmp));
    m1Y = (uSupport[2]-uTorso1[2])/(tStep*ph1Zmp);
    m2Y = (uTorso2[2]-uSupport[2])/(tStep*(1-ph2Zmp));
    aXP, aXN = zmp_solve(uSupport[1], uTorso1[1], uTorso2[1],
    uTorso1[1], uTorso2[1]);
    aYP, aYN = zmp_solve(uSupport[2], uTorso1[2], uTorso2[2],
    uTorso1[2], uTorso2[2]);
  } //} new step

  xFoot, zFoot = foot_phase(ph);  
  if initial_step>0 { zFoot=0;  } //Don't lift foot at initial step
  pLLeg[3], pRLeg[3] = 0;
  if supportLeg == 0 {    // Left support
    if walkKickRequest == 4 and walkKickType>1 { //Side kick
      if xFoot<0.5 { uRight = Util.se2_interpolate(xFoot*2, uRight1, uRight15);
      else uRight = Util.se2_interpolate(xFoot*2-1, uRight15, uRight2);
      }
    else
      uRight = Util.se2_interpolate(xFoot, uRight1, uRight2);
    }
    pRLeg[3] = stepHeight*zFoot;
  else    // Right support
    if walkKickRequest == 4 and walkKickType>1 { //side kick 
      if xFoot<0.5 { uLeft = Util.se2_interpolate(xFoot*2, uLeft1, uLeft15);
      else uLeft = Util.se2_interpolate(xFoot*2-1, uLeft15, uLeft2);      
      }
    else
      uLeft = Util.se2_interpolate(xFoot, uLeft1, uLeft2);
    }
    pLLeg[3] = stepHeight*zFoot;
  }

  uTorso = zmp_com(ph);
  uTorsoActual = Util.pose_global(new List<double>({-footX,0,0}),uTorso);

  pLLeg[1], pLLeg[2], pLLeg[6] = uLeft[1], uLeft[2], uLeft[3];
  pRLeg[1], pRLeg[2], pRLeg[6] = uRight[1], uRight[2], uRight[3];
  pTorso[1], pTorso[2], pTorso[6] = uTorsoActual[1], uTorsoActual[2], uTorsoActual[3];

  qLegs = Kinematics.inverse_legs(pLLeg, pRLeg, pTorso, supportLeg);
  motion_legs(qLegs);
  motion_arms();
}

function check_walkkick()
  //Check walking kick phases
  if walkKickType>1 { return; }


  if walkKickRequest ==1 { //If support foot is right, skip 1st step
    Console.WriteLine("NEWNEWKICK: WALKKICK START")
    if supportLeg==walkKickType { 
      walkKickRequest = 2;
    }
  }

  if walkKickRequest == 1 { 
    // Feet together
    if supportLeg == 0 { uRight2 = Util.pose_global({0,-2*footY,0}, uLeft1); 
    else uLeft2 = Util.pose_global({0,2*footY,0}, uRight1); 
    }
    walkKickRequest = walkKickRequest + 1;

  elseif walkKickRequest ==2 { 
    // Support step forward
    if supportLeg == 0 { 
      uRight2 = Util.pose_global({walkKickVel[1],-2*footY,0}, uLeft1);
      shiftFactor = 0.7; //shift final torso to right foot
    else 
      uLeft2 = Util.pose_global({walkKickVel[1],2*footY,0}, uRight1); 
      shiftFactor = 0.3; //shift final torso to left foot
    }
    supportMod = walkKickSupportMod[1];
    walkKickRequest = walkKickRequest + 1;

    //Slow down tStep for two kick step
    tStep=tStepWalkKick;

  elseif walkKickRequest ==3 { 
    // Kicking step forward
    if supportLeg == 0 { uRight2 = Util.pose_global({walkKickVel[2],-2*footY,0}, uLeft1);
    else uLeft2 = Util.pose_global({walkKickVel[2],2*footY,0}, uRight1);//RS
    }
    supportMod = walkKickSupportMod[2];
    walkKickRequest = walkKickRequest + 1;

  elseif walkKickRequest == 4 { 
    // Feet together
    if supportLeg == 0 { uRight2 = Util.pose_global({0,-2*footY,0}, uLeft1); 
    else uLeft2 = Util.pose_global({0,2*footY,0}, uRight1); 
    }
    walkKickRequest = 0;
    tStep=tStep0; 

  }
}


function update_still()
  uTorso = step_torso(uLeft, uRight,0.5);
  uTorsoActual = Util.pose_global(new List<double>({-footX,0,0}),uTorso);
  pLLeg[1], pLLeg[2], pLLeg[6] = uLeft[1], uLeft[2], uLeft[3];
  pRLeg[1], pRLeg[2], pRLeg[6] = uRight[1], uRight[2], uRight[3];
  pLLeg[3], pRLeg[3] = 0;
  pTorso[1], pTorso[2], pTorso[6] = uTorsoActual[1], uTorsoActual[2], uTorsoActual[3];
  qLegs = Kinematics.inverse_legs(pLLeg, pRLeg, pTorso, supportLeg);
  motion_legs(qLegs);
  motion_arms();
}


function motion_legs(qLegs)
  phComp = math.min(1, phSingle/.1, (1-phSingle)/.1);

  //Ankle stabilization using gyro feedback
  imuGyr = Body.get_sensor_imuGyrRPY();

  gyro_roll0=imuGyr[1];
  gyro_pitch0=imuGyr[2];

  //get effective gyro angle considering body angle offset
  if not active { //double support
    yawAngle = (uLeft[3]+uRight[3])/2-uTorsoActual[3];
  elseif supportLeg == 0 {  // Left support
    yawAngle = uLeft[3]-uTorsoActual[3];
  elseif supportLeg==1 {
    yawAngle = uRight[3]-uTorsoActual[3];
  }
  gyro_roll = gyro_roll0*math.cos(yawAngle) +
  -gyro_pitch0* math.sin(yawAngle);
  gyro_pitch = gyro_pitch0*math.cos(yawAngle)
  -gyro_roll0* math.sin(yawAngle);

  ankleShiftX=Util.procFunc(gyro_pitch*ankleImuParamX[2],ankleImuParamX[3],ankleImuParamX[4]);
  ankleShiftY=Util.procFunc(gyro_roll*ankleImuParamY[2],ankleImuParamY[3],ankleImuParamY[4]);
  kneeShiftX=Util.procFunc(gyro_pitch*kneeImuParamX[2],kneeImuParamX[3],kneeImuParamX[4]);
  hipShiftY=Util.procFunc(gyro_roll*hipImuParamY[2],hipImuParamY[3],hipImuParamY[4]);
  armShiftX=Util.procFunc(gyro_pitch*armImuParamY[2],armImuParamY[3],armImuParamY[4]);
  armShiftY=Util.procFunc(gyro_roll*armImuParamY[2],armImuParamY[3],armImuParamY[4]);

  ankleShift[1]=ankleShift[1]+ankleImuParamX[1]*(ankleShiftX-ankleShift[1]);
  ankleShift[2]=ankleShift[2]+ankleImuParamY[1]*(ankleShiftY-ankleShift[2]);
  kneeShift=kneeShift+kneeImuParamX[1]*(kneeShiftX-kneeShift);
  hipShift[2]=hipShift[2]+hipImuParamY[1]*(hipShiftY-hipShift[2]);
  armShift[1]=armShift[1]+armImuParamX[1]*(armShiftX-armShift[1]);
  armShift[2]=armShift[2]+armImuParamY[1]*(armShiftY-armShift[2]);

  //TODO: Toe/heel lifting
  toeTipCompensation = 0;

  if not active { //Double support, standing still
    //qLegs[2] = qLegs[2] + hipShift[2];    //Hip roll stabilization
    qLegs[4] = qLegs[4] + kneeShift;    //Knee pitch stabilization
    qLegs[5] = qLegs[5]  + ankleShift[1];    //Ankle pitch stabilization
    //qLegs[6] = qLegs[6] + ankleShift[2];    //Ankle roll stabilization

    //qLegs[8] = qLegs[8]  + hipShift[2];    //Hip roll stabilization
    qLegs[10] = qLegs[10] + kneeShift;    //Knee pitch stabilization
    qLegs[11] = qLegs[11]  + ankleShift[1];    //Ankle pitch stabilization
    //qLegs[12] = qLegs[12] + ankleShift[2];    //Ankle roll stabilization

  elseif supportLeg == 0 {  // Left support
    qLegs[2] = qLegs[2] + hipShift[2];    //Hip roll stabilization
    qLegs[4] = qLegs[4] + kneeShift;    //Knee pitch stabilization
    qLegs[5] = qLegs[5]  + ankleShift[1];    //Ankle pitch stabilization
    qLegs[6] = qLegs[6] + ankleShift[2];    //Ankle roll stabilization

    qLegs[11] = qLegs[11]  + toeTipCompensation*phComp;//Lifting toetip
    qLegs[2] = qLegs[2] + hipRollCompensation*phComp; //Hip roll compensation
  else
    qLegs[8] = qLegs[8]  + hipShift[2];    //Hip roll stabilization
    qLegs[10] = qLegs[10] + kneeShift;    //Knee pitch stabilization
    qLegs[11] = qLegs[11]  + ankleShift[1];    //Ankle pitch stabilization
    qLegs[12] = qLegs[12] + ankleShift[2];    //Ankle roll stabilization

    qLegs[5] = qLegs[5]  + toeTipCompensation*phComp;//Lifting toetip
    qLegs[8] = qLegs[8] - hipRollCompensation*phComp;//Hip roll compensation
  }

  //[[
  var spread=(uLeft[3]-uRight[3])/2;
  qLegs[5] = qLegs[5] + 0;//Config.walk.anklePitchComp[1]*math.cos(spread);
  qLegs[11] = qLegs[11] + 0;//Config.walk.anklePitchComp[2]*math.cos(spread);
  //]]

  Body.set_lleg_command(qLegs);
}

function motion_arms()
  var qLArmActual={};   
  var qRArmActual={};   

  if walkKickRequest >2 and walkKickType>1 { //Side kick, wide arm stance
    qLArmActual[1],qLArmActual[2]=qLArmKick0[1]+armShift[1],qLArmKick0[2]+armShift[2];
    qRArmActual[1],qRArmActual[2]=qRArmKick0[1]+armShift[1],qRArmKick0[2]+armShift[2];
  else //Normal arm stance
    qLArmActual[1],qLArmActual[2]=qLArm0[1]+armShift[1],qLArm0[2]+armShift[2];
    qRArmActual[1],qRArmActual[2]=qRArm0[1]+armShift[1],qRArm0[2]+armShift[2];
  }

  qLArmActual[2]=math.max(8*math.pi/180,qLArmActual[2])
  qRArmActual[2]=math.min(-8*math.pi/180,qRArmActual[2]);
  qLArmActual[3]=qLArm0[3];
  qRArmActual[3]=qRArm0[3];
  Body.set_larm_command(qLArmActual);
  Body.set_rarm_command(qRArmActual);
}

function exit()
}

function step_left_destination(vel, uLeft, uRight)
  var u0 = Util.se2_interpolate(.5, uLeft, uRight);
  // Determine nominal midpoint position 1.5 steps in future
  var u1 = Util.pose_global(vel, u0);
  var u2 = Util.pose_global(.5*vel, u1);
  var uLeftPredict = Util.pose_global({0, footY, 0}, u2);
  var uLeftRight = Util.pose_relative(uLeftPredict, uRight);
  // Do not pidgeon toe, cross feet:

  //Check toe and heel overlap
  var toeOverlap= -footSizeX[1]*uLeftRight[3];
  var heelOverlap= -footSizeX[2]*uLeftRight[3];
  var limitY = math.max(stanceLimitY[1],
  stanceLimitY2+math.max(toeOverlap,heelOverlap));

  //Console.WriteLine("Toeoverlap Heeloverlap",toeOverlap,heelOverlap,limitY)

  uLeftRight[1] = math.min(math.max(uLeftRight[1], stanceLimitX[1]), stanceLimitX[2]);
  uLeftRight[2] = math.min(math.max(uLeftRight[2], limitY),stanceLimitY[2]);
  uLeftRight[3] = math.min(math.max(uLeftRight[3], stanceLimitA[1]), stanceLimitA[2]);

  return Util.pose_global(uLeftRight, uRight);
}

function step_right_destination(vel, uLeft, uRight)
  var u0 = Util.se2_interpolate(.5, uLeft, uRight);
  // Determine nominal midpoint position 1.5 steps in future
  var u1 = Util.pose_global(vel, u0);
  var u2 = Util.pose_global(.5*vel, u1);
  var uRightPredict = Util.pose_global({0, -footY, 0}, u2);
  var uRightLeft = Util.pose_relative(uRightPredict, uLeft);
  // Do not pidgeon toe, cross feet:

  //Check toe and heel overlap
  var toeOverlap= footSizeX[1]*uRightLeft[3];
  var heelOverlap= footSizeX[2]*uRightLeft[3];
  var limitY = math.max(stanceLimitY[1],
  stanceLimitY2+math.max(toeOverlap,heelOverlap));

  //Console.WriteLine("Toeoverlap Heeloverlap",toeOverlap,heelOverlap,limitY)

  uRightLeft[1] = math.min(math.max(uRightLeft[1], stanceLimitX[1]), stanceLimitX[2]);
  uRightLeft[2] = math.min(math.max(uRightLeft[2], -stanceLimitY[2]), -limitY);
  uRightLeft[3] = math.min(math.max(uRightLeft[3], -stanceLimitA[2]), -stanceLimitA[1]);

  return Util.pose_global(uRightLeft, uLeft);
}

function step_torso(uLeft, uRight,shiftFactor)
  var u0 = Util.se2_interpolate(.5, uLeft, uRight);
  var uLeftSupport = Util.pose_global({supportX, supportY, 0}, uLeft);
  var uRightSupport = Util.pose_global({supportX, -supportY, 0}, uRight);
  return Util.se2_interpolate(shiftFactor, uLeftSupport, uRightSupport);
}

function set_velocity(vx, vy, vz)
  //Filter the commanded speed
  //[[
  vz= math.min(math.max(vz,velLimitA[1]),velLimitA[2]);
  var stepMag=math.sqrt(vx^2+vy^2);
  var magFactor=math.min(0.10,stepMag)/(stepMag+0.000001);
  //]]

  magFactor = 1;
  velCommand[1]=vx*magFactor;
  velCommand[2]=vy*magFactor;
  velCommand[3]=vz;

  velCommand[1] = math.min(math.max(velCommand[1],velLimitX[1]),velLimitX[2]);
  velCommand[2] = math.min(math.max(velCommand[2],velLimitY[1]),velLimitY[2]);
  velCommand[3] = math.min(math.max(velCommand[3],velLimitA[1]),velLimitA[2]);

}

function update_velocity()
  velDiff[1]= math.min(math.max(velCommand[1]-velCurrent[1],
  -velDelta[1]),velDelta[1]);
  velDiff[2]= math.min(math.max(velCommand[2]-velCurrent[2],
  -velDelta[2]),velDelta[2]);
  velDiff[3]= math.min(math.max(velCommand[3]-velCurrent[3],
  -velDelta[3]),velDelta[3]);

  velCurrent[1] = velCurrent[1]+velDiff[1];
  velCurrent[2] = velCurrent[2]+velDiff[2];
  velCurrent[3] = velCurrent[3]+velDiff[3];

  if initial_step>0 {
    velCurrent=new List<double>({0,0,0})
    initial_step=initial_step-1;
  }
}

function get_velocity()
  return velCurrent;
}

function start()
  stopRequest = 0;
  if (not active) {
    active = true;
    started = false;
    iStep0 = -1;
    t0 = Body.get_time();
    tLastStep = Body.get_time();
    initial_step=2;
  }
}

function stop()
  //Always stops with feet together (which helps kicking)
  stopRequest = math.max(1,stopRequest);
  //  stopRequest = 2; //Stop w/o feet together
}

function stopAlign() //Depreciated, we always stop with feet together 
  stop()
}

function doWalkKickLeft()
  if walkKickRequest==0 {
    walkKickRequest = 1; 
    walkKickType = 0; //Start with left support 
  }
}

function doWalkKickRight()
  if walkKickRequest==0 {
    walkKickRequest = 1; 
    walkKickType = 1; //Start with right support
  }
}




function zmp_solve(zs, z1, z2, x1, x2)
  //[[
  Solves ZMP equation:
  x(t) = z(t) + aP*exp(t/tZmp) + aN*exp(-t/tZmp) - tZmp*mi*sinh((t-Ti)/tZmp)
  where the ZMP point is piecewise linear:
  z(0) = z1, z(T1 < t < T2) = zs, z(tStep) = z2
  //]]
  var T1 = tStep*ph1Zmp;
  var T2 = tStep*ph2Zmp;
  var m1 = (zs-z1)/T1;
  var m2 = -(zs-z2)/(tStep-T2);

  var c1 = x1-z1+tZmp*m1*math.sinh(-T1/tZmp);
  var c2 = x2-z2+tZmp*m2*math.sinh((tStep-T2)/tZmp);
  var expTStep = math.exp(tStep/tZmp);
  var aP = (c2 - c1/expTStep)/(expTStep-1/expTStep);
  var aN = (c1*expTStep - c2)/(expTStep-1/expTStep);
  return aP, aN;
}

//Finds the necessary COM for stability and returns it
function zmp_com(ph)
  var com = new List<double>({0, 0, 0});
  expT = math.exp(tStep*ph/tZmp);
  com[1] = uSupport[1] + aXP*expT + aXN/expT;
  com[2] = uSupport[2] + aYP*expT + aYN/expT;
  if (ph < ph1Zmp) {
    com[1] = com[1] + m1X*tStep*(ph-ph1Zmp)
    -tZmp*m1X*math.sinh(tStep*(ph-ph1Zmp)/tZmp);
    com[2] = com[2] + m1Y*tStep*(ph-ph1Zmp)
    -tZmp*m1Y*math.sinh(tStep*(ph-ph1Zmp)/tZmp);
  elseif (ph > ph2Zmp) {
    com[1] = com[1] + m2X*tStep*(ph-ph2Zmp)
    -tZmp*m2X*math.sinh(tStep*(ph-ph2Zmp)/tZmp);
    com[2] = com[2] + m2Y*tStep*(ph-ph2Zmp)
    -tZmp*m2Y*math.sinh(tStep*(ph-ph2Zmp)/tZmp);
  }
  //com[3] = .5*(uLeft[3] + uRight[3]);
  //Linear speed turning
  com[3] = ph* (uLeft2[3]+uRight2[3])/2 + (1-ph)* (uLeft1[3]+uRight1[3])/2;
  return com;
}

function foot_phase(ph)
  // Computes relative x,z motion of foot during single support phase
  // phSingle = 0: x=0, z=0, phSingle = 1: x=1,z=0
  phSingle = math.min(math.max(ph-ph1Single, 0)/(ph2Single-ph1Single),1);
  var phSingleSkew = phSingle^0.8 - 0.17*phSingle*(1-phSingle);
  var xf = .5*(1-math.cos(math.pi*phSingleSkew));
  var zf = .5*(1-math.cos(2*math.pi*phSingleSkew));

  //hack: vertical takeoff and landing
  //  factor1 = 0.2;
  factor1 = 0;
  factor2 = 0;
  phSingleSkew2 = math.max(
  math.min(1,
  (phSingleSkew-factor1)/(1-factor1-factor2)
  ), 0);
  var xf = .5*(1-math.cos(math.pi*phSingleSkew2));

  //Check for walkkick step
  if walkKickRequest == 4 { 
    zf = zf * walkKickHeightFactor; //Increase step height
    if walkKickType <2 { //Different trajectory for Front walkkick
      var kickN = 1.5; 
      if phSingle<0.5 { xf=kickN*phSingle;
      else xf = (1-kickN)*(2*phSingle-1) + kickN;
      }
    }
  }

  return xf, zf;
}



    }
}
