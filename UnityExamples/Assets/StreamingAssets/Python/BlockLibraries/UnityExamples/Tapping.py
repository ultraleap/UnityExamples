from pysensationcore import *
import sensation_helpers as sh
import Generators

circlePathInstance = createInstance("CirclePath", "CirclePathInstance")
pulserBlock = createInstance("PulseWave", "pulserInstance")

# Create hand-tracked Ripple Sensation from output of TransformPath
tappingSensation = sh.createSensationFromPath("Tapping",
                                                {
                                                    ("t", pulserBlock.t) : (0,0,0),
                                                    ("radius", circlePathInstance.radius) : (0.01, 0.0, 0.0),
                                                    ("pulseLength", pulserBlock.pulseLength) : (0.2, 0.0, 0.0),
                                                    ("repeatGap", pulserBlock.repeatGap) : (0.1, 0.0, 0.0),
                                                    ("min", pulserBlock.min) : (0, 0.0, 0.0),
                                                    ("max", pulserBlock.max) : (1, 0.0, 0.0),
                                                },
                                                output = circlePathInstance.out,
                                                drawFrequency = 70,
                                                renderMode=sh.RenderMode.Loop,
                                                intensity = pulserBlock.out)

setMetaData(tappingSensation.min, "Input-Visibility", False)
setMetaData(tappingSensation.max, "Input-Visibility", False)

setMetaData(tappingSensation.radius, "Type", "Scalar")
setMetaData(tappingSensation.pulseLength, "Type", "Scalar")
setMetaData(tappingSensation.repeatGap, "Type", "Scalar")
setMetaData(tappingSensation, "Allow-Transform", True)
