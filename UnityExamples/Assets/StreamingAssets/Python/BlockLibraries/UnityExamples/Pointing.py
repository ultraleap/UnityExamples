from pysensationcore import *
import sensation_helpers as sh
import HandOperations

# A Lissajous Path Sensation which will be targeted at the fingertip
lissajousPathInstance = createInstance("LissajousPath", "lissajousPath")

# Additional Transfer to ensure Circle is oriented to palm
orientToPalmInstance = createInstance("OrientPathToPalm", "orientToPalm")

connect(lissajousPathInstance.out, orientToPalmInstance.path)

pointingBlock = sh.createSensationFromPath("Pointing",
                                        {
                                            ("width", lissajousPathInstance.sizeX) : (0.015, 0.0, 0.0),
                                            ("length", lissajousPathInstance.sizeY) : (0.03, 0.0, 0.0),
                                            ("shapeX", lissajousPathInstance.paramA) : (3, 0.0, 0.0),
                                            ("shapeY", lissajousPathInstance.paramB) : (2, 0.0, 0.0),
											("palm_direction", orientToPalmInstance.palm_direction) : (0, 0, 0),
                                            ("palm_normal", orientToPalmInstance.palm_normal) : (0, 0, 0),
                                            ("indexFinger_intermediate_position", orientToPalmInstance.offset_position) : (0,0,0)
                                        },
                                        output = orientToPalmInstance.out,
                                        renderMode = sh.RenderMode.Loop,
                                        definedInVirtualSpace = True,
                                        drawFrequency = 40)

setMetaData(pointingBlock.palm_direction, "Input-Visibility", False)
setMetaData(pointingBlock.palm_normal, "Input-Visibility", False)
setMetaData(pointingBlock.indexFinger_intermediate_position, "Input-Visibility", False)
setMetaData(pointingBlock.width, "Type", "Scalar")
setMetaData(pointingBlock.length, "Type", "Scalar")
setMetaData(pointingBlock.shapeX, "Type", "Scalar")
setMetaData(pointingBlock.shapeY, "Type", "Scalar")
