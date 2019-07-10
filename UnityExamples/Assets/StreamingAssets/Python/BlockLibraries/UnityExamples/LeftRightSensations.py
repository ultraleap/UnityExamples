from pysensationcore import *
import sensation_helpers as sh
import Mux
import Generators
import HandOperations

pointCount = 6
# We have two paths for left and right hands
leftPath = createInstance("PolylinePath", "leftPath")

leftPoints = sh.createList(pointCount)

rightPath = createInstance("PolylinePath", "rightPath")
rightPoints = sh.createList(pointCount)

# We have three modes - Left, Right, or Strobe Left/Right
pathSelectorMux = createInstance("Mux3", "mux")

# A Mux2 to do the Strobing Logic
strobeMux = createInstance("Mux2", "mux2")

# A wave which oscilates between 0 and 1 with a given period
onOffWave = createInstance("BinaryWave", "square")

# connect the output of the BinaryBlock to the StrobeMux
connect(onOffWave.out, strobeMux.selector)

# Connect the point lists to the 'L' and 'R' paths
connect(leftPoints["output"], leftPath.points)
connect(rightPoints["output"], rightPath.points)

# connect the two Paths to the StrobeMux
connect(leftPath.out, strobeMux.input0)
connect(rightPath.out, strobeMux.input1)

# The Leap Data Provide has special inputs "leftHand_present", "rightHand_present"
connect(leftPath.out, pathSelectorMux.input0)
connect(rightPath.out, pathSelectorMux.input1)
connect(strobeMux.out, pathSelectorMux.input2)

selectorInstance = createInstance("HandInfo", "selectorInstance")

# Connect the outputs to the pathSelectorMux
connect(selectorInstance.option, pathSelectorMux.selector)

leftRightSensation = sh.createSensationFromPath("LeftRightTest",
                                      {
                                      	  ("t", onOffWave.t) : (0, 0, 0),
                                      	  ("handSwitchingPeriod", onOffWave.period) : (0.05, 0, 0),
                                          ("leftHand_present", selectorInstance.leftHand_present) : (0, 0, 0),
                                          ("rightHand_present", selectorInstance.rightHand_present) : (0, 0, 0),
                                          ("leftHand_middleFinger_distal_position", leftPoints["inputs"][0]) : (0.03450891, 0.06073823, 0.0),
                                          ("leftHand_middleFinger_distal_position", leftPoints["inputs"][1]) : (0.03450891, -0.04627897, 0.0),
                                          ("leftHand_middleFinger_distal_position", leftPoints["inputs"][2]) : (-0.04455937, -0.04627897, 0.0),
                                          ("leftHand_middleFinger_distal_position", leftPoints["inputs"][3]) : (0.03450891, 0.06073823, 0.0),
                                          ("leftHand_wrist_position", leftPoints["inputs"][4]) : (0.03450891, -0.04627897, 0.0),
                                          ("leftHand_thumb_distal_position", leftPoints["inputs"][5]) : (-0.04455937, -0.04627897, 0.0),
                                          ("rightHand_wrist_position", rightPoints["inputs"][0]) : (-0.02427676, -0.0573265, 0.0),
                                          ("rightHand_palm_position", rightPoints["inputs"][1]) : (-0.02427676, -0.004362214, 0.0),
                                          ("rightHand_middleFinger_distal_position", rightPoints["inputs"][2]) : (0.02363421, -0.004362214, 0.0),
                                          ("rightHand_pinkyFinger_distal_position", rightPoints["inputs"][3]) : (0.02350275, -0.0568287, 0.0),
                                          ("rightHand_palm_position", rightPoints["inputs"][4]) : (0.02436836, 0.05879875, 0.0),
                                          ("rightHand_pinkyFinger_metacarpal_position", rightPoints["inputs"][5]) : (-0.02427676, -0.003085963, 0.0)
                                      },
                                      output = pathSelectorMux.out,
                                      drawFrequency = 70,
                                      definedInVirtualSpace = True
                                      )

setMetaData(leftRightSensation.handSwitchingPeriod, "Type", "Scalar")
setMetaData(leftRightSensation.leftHand_present, "Type", "Scalar")
setMetaData(leftRightSensation.rightHand_present, "Type", "Scalar")
setMetaData(leftRightSensation.leftHand_present, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_present, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_palm_position, "Input-Visibility", False)
setMetaData(leftRightSensation.leftHand_middleFinger_distal_position, "Input-Visibility", False)
setMetaData(leftRightSensation.leftHand_wrist_position, "Input-Visibility", False)
setMetaData(leftRightSensation.leftHand_thumb_distal_position, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_wrist_position, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_palm_position, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_middleFinger_distal_position, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_pinkyFinger_distal_position, "Input-Visibility", False)
setMetaData(leftRightSensation.rightHand_pinkyFinger_metacarpal_position, "Input-Visibility", False)