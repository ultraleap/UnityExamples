from pysensationcore import *
import QuadToQuadIntersection
import sensation_helpers as sh

# === Forcefield  ===

quadToQuadIntersectionBlockInstance = createInstance("QuadToQuadIntersection", "quadToQuadIntersectionInstance")
linePathInstance = createInstance("LinePath", "line")

connect(quadToQuadIntersectionBlockInstance.endpointA, linePathInstance.endpointA)
connect(quadToQuadIntersectionBlockInstance.endpointB, linePathInstance.endpointB)

forcefieldBlock = sh.createSensationFromPath("ForcefieldLine",
                                             {
                                             ("palm_position", quadToQuadIntersectionBlockInstance.center0) : (0.0, 0.0, 0.0),
                                             ("palm_scaled_direction", quadToQuadIntersectionBlockInstance.up0) : (0.0, 0.0, 0.0),
                                             ("palm_scaled_transverse", quadToQuadIntersectionBlockInstance.right0) : (0.0, 0.0, 0.0),

                                             ("forcefieldCenter", quadToQuadIntersectionBlockInstance.center1) : (0.0, 0.1, 0.0),
                                             ("forcefieldUp", quadToQuadIntersectionBlockInstance.up1) : (0.0, 0.1, 0.0),
                                             ("forcefieldRight", quadToQuadIntersectionBlockInstance.right1) : (0.1, 0.0, 0.0)
                                             },
                                             output = linePathInstance.out,
                                             definedInVirtualSpace = True
                                             )

# ForcefieldLine IS strictly a Sensation-Producing Block 
# To avoid clutter in the Sensation-Producing Block menu, we only show the two-handed version 'Forcefield' (defined below)
setMetaData(forcefieldBlock.out, "Sensation-Producing", False)

setMetaData(forcefieldBlock.palm_position, "Input-Visibility", False)
setMetaData(forcefieldBlock.palm_scaled_direction, "Input-Visibility", False)
setMetaData(forcefieldBlock.palm_scaled_transverse, "Input-Visibility", False)

setMetaData(forcefieldBlock.palm_position, "Docs.Description", "Center of the palm in virtual space")
setMetaData(forcefieldBlock.palm_scaled_direction, "Docs.Description", "Direction of palm. The size of the vector is half the length of the hand.")
setMetaData(forcefieldBlock.palm_scaled_transverse, "Docs.Description", "Vector perpendicular palm_length. The size of the vector is half the width of the hand.")

setMetaData(forcefieldBlock.forcefieldCenter, "Docs.Description", "Center of the forcefield in virtual space")
setMetaData(forcefieldBlock.forcefieldUp, "Docs.Description", "Direction of the forcefield. The size of this vector is half the length of the force field.")
setMetaData(forcefieldBlock.forcefieldRight, "Docs.Description", "Vector perpendicular to forcefieldUp. The size of this vector is half the width of the force field.")


from TwoHandedSensation import *

# Making the Forcefield work for two hands
forcefieldInst = createInstance("ForcefieldLine", "Inst")
twoHandedForcefield = makeSensationTwoHanded(forcefieldInst, "Forcefield")

unconnectedInputs = ["forcefieldCenter", "forcefieldUp", "forcefieldRight"]

defineInputs(twoHandedForcefield, *unconnectedInputs)
for input in unconnectedInputs:
    connect(getattr(twoHandedForcefield, input), getattr(forcefieldInst, input))

defineBlockInputDefaultValue(twoHandedForcefield.forcefieldCenter, (0, 0.1, 0))
defineBlockInputDefaultValue(twoHandedForcefield.forcefieldUp, (0, 0.1, 0))
defineBlockInputDefaultValue(twoHandedForcefield.forcefieldRight, (0.1, 0, 0))
defineBlockInputDefaultValue(twoHandedForcefield.drawFrequency, (100, 0, 0))
setMetaData(twoHandedForcefield.handSwitchingPeriod, "Input-Visibility", False)

defineOutputs(twoHandedForcefield, "out")
connect(getattr(forcefieldInst, "out"), getattr(twoHandedForcefield, "out"))