//
//  LTBitmapHandleConverter.h
//  Leadtools.Converters
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#include <CoreGraphics/CGImage.h>

#include <Leadtools.Converters/LTConvertersDefines.h>
#include <Leadtools/Kernel/ltkrn.h>
#include <Leadtools/LTLeadtools.h>

typedef struct {
    L_UINT structSize;
    L_UINT maximumWidth;
    L_UINT maximumHeight;
    L_UINT sizeMode;
    L_BOOL resample;
    
    LTConvertToImageOptions options;
} L_ConvertToImageData;

typedef struct {
    L_UINT structSize;
    
    LTConvertFromImageOptions options;
} L_ConvertFromImageData;

LEADTOOLS_EXPORT L_ConvertToImageData _defaultConvertToImageData;
LEADTOOLS_EXPORT L_ConvertFromImageData _defaultConvertFromImageData;

LEADTOOLS_EXPORT L_INT L_ConvertToCGImage(BITMAPHANDLE *bitmapHandle, CGImageRef *image, const L_ConvertToImageData *data);
LEADTOOLS_EXPORT L_INT L_ConvertFromCGImage(CGImageRef image, BITMAPHANDLE *bitmapHandle, const L_ConvertFromImageData *data);
