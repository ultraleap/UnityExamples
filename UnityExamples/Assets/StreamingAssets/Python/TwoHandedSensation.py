from pysensationcore import *
from sensation_helpers import topLevelBlockInputs

listOfFingers = ["thumb", "indexFinger", "middleFinger", "ringFinger", "pinkyFinger"]
listOfBones = ["metacarpal", "intermediate", "proximal", "distal"]
listOfFeatures = ["position"]

listOfHandFeatures = ["palm_position", "palm_direction", "palm_normal", "palm_scaled_transverse", "palm_scaled_direction", "wrist_position"]

def _isInputDefined(block, input):
    try:
        getattr(block, input)
        return True
    except:
        return False


def getHandRelatedInputs(sensation):
    listOfHandRelatedInputs = []
    for finger in listOfFingers:
        for bone in listOfBones:
            for feature in listOfFeatures:
                identifer = finger + "_" + bone + "_" + feature
                if _isInputDefined(sensation, identifer):
                    listOfHandRelatedInputs.append(identifer)

    for identifer in listOfHandFeatures:
        if _isInputDefined(sensation, identifer):
            listOfHandRelatedInputs.append(identifer)

    return listOfHandRelatedInputs


def forwardAnyInputsCreatedViaSensationHelpers(topLevelBlock, innerBlock):
    for input in topLevelBlockInputs:
        inputName = input["name"]
        if _isInputDefined(innerBlock, inputName):
            if not _isInputDefined(topLevelBlock, inputName):
                defineInputs(topLevelBlock, inputName)
                inputHandle = getattr(topLevelBlock, inputName)

                setMetaData(inputHandle, "Input-Visibility", input["input-visibility"])
                if input["type"]:
                    setMetaData(inputHandle, "Type", input["type"])
                if input["default"]:
                    defineBlockInputDefaultValue(inputHandle, input["default"])

            connect(getattr(topLevelBlock, inputName), getattr(innerBlock, inputName))


def makeSensationTwoHanded(sensation, sensationName):
    listOfGenericHandRelatedInputs = getHandRelatedInputs(sensation)
    leftHandInputs = ["leftHand_" + identifier for identifier in listOfGenericHandRelatedInputs]
    rightHandInputs = ["rightHand_" + identifier for identifier in listOfGenericHandRelatedInputs]

    twoHandedSensation = defineBlock(sensationName)
    twoHandedMuxInst = createInstance("TwoHandsMux", sensationName + "TwoHandsMuxInst")

    defineInputs(twoHandedSensation, "t")
    connect(twoHandedSensation.t, twoHandedMuxInst.t)

    defineInputs(twoHandedSensation, "handSwitchingPeriod")
    connect(twoHandedSensation.handSwitchingPeriod, twoHandedMuxInst.handSwitchingPeriod)
    setMetaData(twoHandedSensation.handSwitchingPeriod, "Type", "Scalar")
    defineBlockInputDefaultValue(twoHandedSensation.handSwitchingPeriod, (0.01, 0, 0))

    defineInputs(twoHandedSensation, "leftHand_present", "rightHand_present")
    connect(twoHandedSensation.leftHand_present, twoHandedMuxInst.leftHand_present)
    connect(twoHandedSensation.rightHand_present, twoHandedMuxInst.rightHand_present)
    setMetaData(twoHandedSensation.leftHand_present, "Input-Visibility", False)
    setMetaData(twoHandedSensation.rightHand_present, "Input-Visibility", False)

    twoHandsInputs = leftHandInputs + rightHandInputs
    defineInputs(twoHandedSensation, *twoHandsInputs)
    for identifier in twoHandsInputs:
        connect(getattr(twoHandedSensation, identifier), getattr(twoHandedMuxInst, identifier))
        setMetaData(getattr(twoHandedSensation, identifier), "Input-Visibility", False)

    for identifier in listOfGenericHandRelatedInputs:
        connect(getattr(twoHandedMuxInst, identifier), getattr(sensation, identifier))

    forwardAnyInputsCreatedViaSensationHelpers(twoHandedSensation, sensation)

    return twoHandedSensation
