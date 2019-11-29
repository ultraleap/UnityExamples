#pragma once

#include "SensationCore.h"

#include <memory>
#include <typeindex>
#include <type_traits>
#include <unordered_map>

#include <iostream>

namespace SensationCore {

struct Block {
    uhsclHandle_t handle;
};

class SensationCoreLibrary
{
    using SensationCoreLibraryImplPtr =
            std::unique_ptr<uhsclInstance_t, decltype(&uhsclRelease)>;

public:
    explicit SensationCoreLibrary()
        : impl_(SensationCoreLibraryImplPtr(uhsclCreate(), &uhsclRelease))
    {
        std::cerr << "SensationCoreLibrary constructor\n";
    }
    ~SensationCoreLibrary() = default;

    uhsclInstance impl()
    {
        return impl_.get();
    }

public:
    using BlockRegistry = std::unordered_map<std::type_index,
            std::unique_ptr<Block>>;
    BlockRegistry registeredBlocks;

private:
    SensationCoreLibraryImplPtr impl_;
};

} // namespace SensationCore
