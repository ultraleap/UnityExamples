#pragma once

#include "BlockBuilder.hpp"
#include "uhsclHandle.h"

namespace SensationCore {
namespace BlockLibrary {

struct ComposeTransform : Block {
    uhsclHandle_t transverse;
    uhsclHandle_t direction;
    uhsclHandle_t normal;
    uhsclHandle_t offset;

    uhsclHandle_t out;
};

struct TransformPath : Block {
    uhsclHandle_t path;
    uhsclHandle_t transform;

    uhsclHandle_t out;
};

struct RenderPath : Block {
    uhsclHandle_t path;
    uhsclHandle_t t;
    uhsclHandle_t drawFrequency;

    uhsclHandle_t out;
};

struct SetIntensity : Block {
    uhsclHandle_t point;
    uhsclHandle_t intensity;

    uhsclHandle_t out;
};

} // namespace BlockLibrary

template<>
void buildBlock<BlockLibrary::ComposeTransform>(BlockBuilder builder,
                                                BlockLibrary::ComposeTransform *block)
{
    block->handle = builder.findBlock("ComposeTransform");
    block->transverse = builder.getInputAtIndex(block->handle, 0);
    block->direction = builder.getInputAtIndex(block->handle, 1);
    block->normal = builder.getInputAtIndex(block->handle, 2);
    block->offset = builder.getInputAtIndex(block->handle, 3);

    block->out = builder.getOutputAtIndex(block->handle, 0);
}

template<>
void buildBlock<BlockLibrary::TransformPath>(BlockBuilder builder,
                                             BlockLibrary::TransformPath *block)
{
    block->handle = builder.findBlock("TransformPath");
    block->path = builder.getInputAtIndex(block->handle, 0);
    block->transform = builder.getInputAtIndex(block->handle, 1);

    block->out = builder.getOutputAtIndex(block->handle, 0);
}

template<>
void buildBlock<BlockLibrary::RenderPath>(BlockBuilder builder,
                                          BlockLibrary::RenderPath *block)
{
    block->handle = builder.findBlock("RenderPath");
    block->path = builder.getInputAtIndex(block->handle, 0);
    block->t = builder.getInputAtIndex(block->handle, 1);
    block->drawFrequency = builder.getInputAtIndex(block->handle, 2);

    block->out = builder.getOutputAtIndex(block->handle, 0);
}

template<>
void buildBlock<BlockLibrary::SetIntensity>(BlockBuilder builder,
                                            BlockLibrary::SetIntensity *block)
{
    block->handle = builder.findBlock("SetIntensity");
    block->point = builder.getInputAtIndex(block->handle, 0);
    block->intensity = builder.getInputAtIndex(block->handle, 1);

    block->out = builder.getOutputAtIndex(block->handle, 0);
}

} // namespace SensationCore
