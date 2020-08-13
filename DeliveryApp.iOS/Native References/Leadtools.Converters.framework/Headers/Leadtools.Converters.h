//
//  Leadtools.Converters.h
//  Leadtools.Converters
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#if !defined(LEADTOOLS_CONVERTERS_FRAMEWORK)
#define LEADTOOLS_CONVERTERS_FRAMEWORK

#import <ImageIO/ImageIO.h>

#if defined(LEADTOOLS_FRAMEWORK)
#   import <Leadtools.Converters/LTConvertersDefines.h>
#   import <Leadtools.Converters/LTConvertToImageData.h>
#   import <Leadtools.Converters/LTConvertFromImageData.h>
#   import <Leadtools.Converters/LTRasterImageConverter.h>
#
#   if __has_include(<Leadtools.Converters/LTRasterImageRenderer.h>)
#       import <Leadtools.Converters/LTRasterImageRenderOptions.h>
#       import <Leadtools.Converters/LTRasterImageRenderer.h>
#   endif // #if __has_include(<Leadtools.Converters/LTRasterImageRenderer.h>)
#endif // #if defined(LEADTOOLS_FRAMEWORK)

#if defined(LEADTOOLS_KERNEL_FRAMEWORK)
#   include <Leadtools.Converters/LTConvertersDefines.h>
#   include <Leadtools.Converters/LTBitmapHandleConverter.h>
#endif // #if defined(LEADTOOLS_KERNEL_FRAMEWORK)

// Versioning
#import <Leadtools/LTLeadtools.h>

LEADTOOLS_EXPORT const unsigned char LeadtoolsConvertersVersionString[];
LEADTOOLS_EXPORT const double LeadtoolsConvertersVersionNumber;

#endif // #if !defined(LEADTOOLS_CONVERTERS_FRAMEWORK)
