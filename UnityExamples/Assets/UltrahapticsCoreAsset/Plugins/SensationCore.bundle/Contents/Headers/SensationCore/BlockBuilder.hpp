#pragma once

#include "uhsclHandle.h"
#include "SensationCoreLibrary.hpp"
#include "SensationCore.h"

namespace SensationCore {

template<typename Function, typename... Targs>
auto call(const char *fnName, SensationCoreLibrary &scl, Function fn, Targs... args) {
    const auto &returnCode =
            fn(scl.impl(), std::forward<Targs>(args)...);
    if (NoError != returnCode) {
        throw std::runtime_error(std::string{fnName}
                                 + " failed with code "
                                 + std::to_string(returnCode));
    }
}

class BlockBuilder {
public:
    using Behaviour = std::function<uhsclValue_t(uhsclEvaluationContextHandle)>;

    BlockBuilder(SensationCoreLibrary &scl)
        : scl_(scl)
    {};
    ~BlockBuilder() = default;

    uhsclHandle_t defineBlock(const char *name) const;
    uhsclHandle_t defineBlockInput(uhsclHandle_t blockHandle,
                                   const char *inputName) const;
    uhsclHandle_t defineBlockOutput(uhsclHandle_t blockHandle,
                                    const char *outputName) const;
    void defineBlockOutputBehaviour(uhsclHandle_t outputHandle,
                                    Behaviour behaviour);
    uhsclHandle_t findBlock(const char *name) const;
    uhsclHandle_t getInputAtIndex(uhsclHandle_t blockHandle,
                                  int index) const;
    uhsclHandle_t getOutputAtIndex(uhsclHandle_t blockHandle,
                                   int index) const;
    uhsclBlockInstanceHandle createBlockInstance(uhsclHandle_t blockHandle,
                                                 const char *instanceName) const;

    void connect(uhsclInputPortHandle source,
                 uhsclInputInstanceHandle destination) const
    {
        call(__FUNCTION__, scl_, &uhsclConnectParentInputToChildInput,
             source, destination);
    }

    void connect(uhsclOutputInstanceHandle source,
                 uhsclInputInstanceHandle destination) const
    {
        call(__FUNCTION__, scl_, &uhsclConnectChildren,
             source, destination);
    }

    void connect(uhsclOutputInstanceHandle source,
                 uhsclOutputPortHandle destination) const
    {
        call(__FUNCTION__, scl_, &uhsclConnectChildOutputToParentOutput,
             source, destination);
    }

    SensationCoreLibrary &sensationCore() const;

private:
    SensationCoreLibrary &scl_;
};

uhsclHandle_t BlockBuilder::defineBlock(const char *name) const
{
    uhsclHandle_t blockHandle{};
    call(__FUNCTION__, scl_, &uhsclDefineBlock,
         name, &blockHandle);
    return blockHandle;
}

uhsclHandle_t BlockBuilder::defineBlockInput(uhsclHandle_t blockHandle,
                                             const char *inputName) const
{
    uhsclHandle_t inputHandle{};
    call(__FUNCTION__, scl_, &uhsclDefineBlockInput,
         blockHandle, inputName, &inputHandle);
    return inputHandle;
}

uhsclHandle_t BlockBuilder::defineBlockOutput(uhsclHandle_t blockHandle,
                                              const char *outputName) const
{
    uhsclHandle_t outputHandle{};
    call(__FUNCTION__, scl_, &uhsclDefineBlockOutput,
         blockHandle, outputName, &outputHandle);
    return outputHandle;
}

uhsclVector3_t behaviourWrapper(void *data, uhsclEvaluationContextHandle context)
{
    const auto behaviour = static_cast<BlockBuilder::Behaviour *>(data);
    // TODO: Return result
    return ISensationInputSource::getValueAs<uhsclVector3_t>((*behaviour)(context));
}

static std::vector<std::unique_ptr<BlockBuilder::Behaviour>> behaviours_g;

// TODO: lifetime of behaviour could be fiddly
void BlockBuilder::defineBlockOutputBehaviour(uhsclHandle_t outputHandle,
                                              Behaviour behaviour)
{
    auto behaviourPtr = std::make_unique<Behaviour>(behaviour);
    call(__FUNCTION__, scl_, &uhsclDefineBlockOutputBehaviour,
         outputHandle, behaviourPtr.get(), behaviourWrapper);
    behaviours_g.push_back(std::move(behaviourPtr));
}

uhsclHandle_t BlockBuilder::findBlock(const char *name) const
{
    uhsclHandle_t blockHandle{};
    // TODO: Rename uhsclCreateBlock to uhsclFindBlock
    call(__FUNCTION__, scl_, &uhsclCreateBlock, name, &blockHandle);
    return blockHandle;
}

uhsclHandle_t BlockBuilder::getInputAtIndex(uhsclHandle_t blockHandle,
                                            int index) const
{
    uhsclHandle_t inputHandle{};
    call(__FUNCTION__, scl_, &uhsclGetInputAtIndex,
         blockHandle, index, &inputHandle);
    return inputHandle;
}

uhsclHandle_t BlockBuilder::getOutputAtIndex(uhsclHandle_t blockHandle,
                                             int index) const
{
    uhsclHandle_t outputHandle{};
    call(__FUNCTION__, scl_, &uhsclGetOutputAtIndex,
         blockHandle, index, &outputHandle);
    return outputHandle;
}

uhsclBlockInstanceHandle BlockBuilder::createBlockInstance(uhsclHandle_t blockHandle,
                                                           const char *instanceName) const
{
    uhsclBlockInstanceHandle instanceHandle;
    call(__FUNCTION__, scl_, &uhsclCreateBlockInstance,
         blockHandle, instanceName, &instanceHandle);
    return instanceHandle;
}

SensationCoreLibrary &BlockBuilder::sensationCore() const
{
    return scl_;
}

template<class T>
void buildBlock(BlockBuilder builder, T *block)
{}

} // namespace SensationCore
