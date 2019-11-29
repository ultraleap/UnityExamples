#pragma once

#include "BlockBuilder.hpp"
#include "SensationCoreLibrary.hpp"
#include "uhsclHandle.h"
#include "uhsclVector.h"

#include <exception>
#include <typeindex>
#include <typeinfo>
#include <unordered_map>

namespace SensationCore {

template<typename T>
void registerBlock(SensationCoreLibrary &scl)
{
    const auto &builder = BlockBuilder(scl);
    auto block = std::make_unique<T>();
    block->handle = builder.defineBlock(typeid(T).name());
    SensationCore::buildBlock<T>(builder, block.get());
    scl.registeredBlocks[std::type_index(typeid(T))] = std::move(block);
}

template<typename T>
struct BlockInstance {
    const T *block;
    uhsclBlockInstanceHandle handle;

    uhsclInputInstanceHandle input(uhsclHandle_t T::*port) const {
        return uhsclInputInstance(handle, block->*port);
    }

    uhsclOutputInstanceHandle output(uhsclHandle_t T::*port) const {
        return uhsclOutputInstance(handle, block->*port);
    }
};

template<typename T>
auto createBlockInstance(BlockBuilder builder)
{
    auto &scl = builder.sensationCore();
    auto &registeredBlocks = scl.registeredBlocks;
    const auto &it0 = registeredBlocks.find(std::type_index(typeid(T)));
    if (it0 == registeredBlocks.end())
    {
        registerBlock<T>(scl);
    }

    const auto &it1 = registeredBlocks.find(std::type_index(typeid(T)));
    if (it1 == registeredBlocks.end())
    {
        std::cerr << "Requested instance of an unregistered block type\n";
        throw std::runtime_error("Requested instance of an unregistered block type");
    }
    else
    {
        BlockInstance<T> instance;
        // TODO: Downcast / static_cast - bad smell?
        // TYPE info across compilation units?
        instance.block = static_cast<const T*>(it1->second.get());
        // TODO: unique instance names?
        instance.handle = builder.createBlockInstance(instance.block->handle, typeid(T).name());
        return instance;
    }
}

} // namespace SensationCore
