from pysensationcore import *
import sensation_helpers as sh
from TwoHandedSensation import *

prefix = "point"
polylinePath = createInstance("PolylinePath", "PolylinePathInstance")
points = sh.createList(6)
connect(points["output"], polylinePath.points)

pinch = sh.createSensationFromPath("PinchBase",
                        {
                            ("thumb_distal_position", points["inputs"][0]) : (0,0,0),
                            ("thumb_intermediate_position", points["inputs"][1]) : (0,0,0),
                            ("thumb_metacarpal_position", points["inputs"][2]) : (0,0,0),
                            ("indexFinger_metacarpal_position", points["inputs"][3]) : (0,0,0),
                            ("indexFinger_proximal_position", points["inputs"][4]) : (0,0,0),
                            ("indexFinger_distal_position", points["inputs"][5]) : (0,0,0)
                        },
                        output = polylinePath.out,
                        drawFrequency = 140,
                        definedInVirtualSpace = True,
                        renderMode = sh.RenderMode.Bounce
                        )
                        
setMetaData(pinch.thumb_distal_position, "Input-Visibility", False)                        
setMetaData(pinch.thumb_intermediate_position, "Input-Visibility", False)
setMetaData(pinch.thumb_metacarpal_position, "Input-Visibility", False)
setMetaData(pinch.indexFinger_metacarpal_position, "Input-Visibility", False)
setMetaData(pinch.indexFinger_proximal_position, "Input-Visibility", False)
setMetaData(pinch.indexFinger_distal_position, "Input-Visibility", False)
setMetaData(pinch.out, "Sensation-Producing", False)
                        
# Two handed Pinch
oneHandedPinchInst = createInstance("PinchBase", "Inst")
twoHandedPinch = makeSensationTwoHanded(oneHandedPinchInst, "Pinch")

defineOutputs(twoHandedPinch, "out")
connect(getattr(oneHandedPinchInst, "out"), getattr(twoHandedPinch, "out"))