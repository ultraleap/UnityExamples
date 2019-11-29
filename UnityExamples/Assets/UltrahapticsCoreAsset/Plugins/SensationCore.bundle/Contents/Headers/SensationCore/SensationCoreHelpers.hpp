#pragma once

#include "BlockEvaluation.hpp"
#include "BlockBuilder.hpp"
#include "BlockDefinition.hpp"

#include "SensationCoreLibrary.hpp"

#include <atomic>
#include <chrono>
#include <memory>
#include <thread>

namespace SensationCore {

void runUntil(uhsclInstance scl,
              const EvaluationOutput &output,
              const std::atomic<bool> *terminating)
{
    if (NoError != uhsclAcquireEmitter(scl))
    {
        throw std::runtime_error("bad uhsclAcquireEmitter");
    }
    uhsclHandle_t playbackDevice;
    if (NoError != uhsclStart(scl, output.port, output.evaluation->inputSourceHandle_, &playbackDevice))
    {
        throw std::runtime_error("bad uhsclStart");
    }
    if (NoError != uhsclUnmute(scl, playbackDevice))
    {
        throw std::runtime_error("bad uhsclUnmute");
    }

    while (*terminating == false)
    {

        // TODO: Need ability to see output value
        // TODO: I need help profiling my code...I'm getting clicking from the array.
        // TODO: RelWithDebInfo
        // TODO: Noisy Time advanced Nms beyond the end of the buffer is annoying sometimes.
        // TODO: check return code
        for (const auto &values : output.evaluation->valueSources_)
        {
            const auto &port = values.first;
            const auto &valueSource = values.second;
            const auto value = valueSource();
            output.evaluation->setPortValue(port, value);
        }
        if (NoError != uhsclUpdate(scl))
        {
            throw std::runtime_error("bad uhsclUpdate");
        }

        using namespace std::chrono_literals;
        std::this_thread::sleep_for(1s / 60.0);
    }
    uhsclStop(scl, playbackDevice);
    uhsclReleaseEmitter(scl);
}

namespace Value {
using Source = std::function<SensationCore::uhsclValue_t()>;
using Vector = uhsclVector3_t;
using Scalar = double;

template<typename T>
T as(const SensationCore::uhsclValue_t &value)
{
    return SensationCore::ISensationInputSource::getValueAs<T>(value);
}

template<>
Scalar as(const SensationCore::uhsclValue_t &value)
{
    if (ISensationInputSource::matchesType<Vector>(value))
    {
        return ISensationInputSource::getValueAs<uhsclVector3_t>(value).x;
    }
    else
    {
        return ISensationInputSource::getValueAs<Scalar>(value);
    }
}

}

} // namespace SensationCore

