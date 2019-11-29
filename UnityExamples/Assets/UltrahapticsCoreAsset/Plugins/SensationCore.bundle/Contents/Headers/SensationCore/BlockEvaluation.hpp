#pragma once

#include "SensationCoreLibrary.hpp"
#include "uhsclHandle.h"
#include "Value.hpp"
#include "ISensationInputSource.hpp"

#include <atomic>
#include <exception>
#include <memory>
#include <typeindex>
#include <functional>

#include <iostream>

namespace SensationCore {

class BlockEvaluation;

struct EvaluationOutput
{
    uhsclHandle_t port;
    BlockEvaluation *evaluation;
};

class PortValueSetter
{
public:
    PortValueSetter(BlockEvaluation *evaluation, uhsclHandle_t port)
            : evaluation_(evaluation)
            , port_(port)
    {}

    void operator=(const SensationCore::uhsclValue_t &);

private:
    BlockEvaluation *evaluation_;
    uhsclHandle_t port_;
};

class BlockEvaluationImpl;

class BlockEvaluation {

public:
    BlockEvaluation(SensationCoreLibrary &scl, const Block &block)
            : scl_(scl)
            , block_(block.handle)
    {
        std::cout << "Creating BlockEvaluation\n";
        uhsclCreateInputSource(scl_.impl(), block.handle, &inputSourceHandle_);
        std::cout << "Created BlockEvaluation\n";
    }

    ~BlockEvaluation() = default;

    void setPortValue(uhsclHandle_t port, const SensationCore::uhsclValue_t &value)
    {
        if (SensationCore::ISensationInputSource::matchesType<std::function<SensationCore::uhsclValue_t()>>(value))
        {
            valueSources_[port] = SensationCore::ISensationInputSource::getValueAs<std::function<SensationCore::uhsclValue_t()>>(value);
        }
        else
        {
            uhsclSetInputToUhsclVector3(scl_.impl(), inputSourceHandle_, port, ISensationInputSource::getValueAs<uhsclVector3_t>(value));
        }
    }

    PortValueSetter input(uhsclHandle_t port);
    EvaluationOutput output(uhsclHandle_t port);

private:
    SensationCoreLibrary &scl_;
    uhsclHandle_t inputSourceHandle_;
    uhsclHandle_t block_;
    std::unordered_map<uhsclHandle_t, std::function<SensationCore::uhsclValue_t()>> valueSources_;

    friend void runUntil(uhsclInstance scl, const EvaluationOutput &output, const std::atomic<bool> *terminating);
};

template<typename T>
std::unique_ptr<BlockEvaluation> createBlockEvaluation(SensationCoreLibrary &scl)
{
    const auto &registeredBlocks = scl.registeredBlocks;
    const auto &it = registeredBlocks.find(std::type_index(typeid(T)));
    if (it == registeredBlocks.end())
    {
        throw std::runtime_error("Requested evaluation of an unregistered block type");
    }
    else
    {
        // TODO: Langauage boundary pain
        std::cout << "About to create BlockEvaluation\n";
        return std::make_unique<BlockEvaluation>(scl, *(it->second));
    }
}

inline PortValueSetter BlockEvaluation::input(uhsclHandle_t port)
{
    return PortValueSetter(this, port);
}

inline EvaluationOutput BlockEvaluation::output(uhsclHandle_t port)
{
    EvaluationOutput output;
    output.port = port;
    output.evaluation = this;
    return output;
}

inline void PortValueSetter::operator=(const uhsclValue_t &value)
{
    evaluation_->setPortValue(port_, value);
}

}
