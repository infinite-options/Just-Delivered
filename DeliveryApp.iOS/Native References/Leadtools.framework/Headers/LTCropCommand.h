//
//  LTCropCommand.h
//  Leadtools
//
//  Copyright (c) 1991-2020 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_ASSUME_NONNULL_BEGIN

/**
 @brief Crops the image, reulting in an image that is the size of the specified rectangle.
 */
NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCropCommand : LTRasterCommand

/** @brief The rectangle area to which the image will be cropped. */
@property (nonatomic, assign) LeadRect rectangle;


/**
 @brief Initializes a new LTCropCommand with explicit parameters.
 
 @param rect The rectangle area to which the image will be cropped.
 
 @returns A LTCropCommand object initialized with specific values.
 */
- (instancetype)initWithRectangle:(LeadRect)rect NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
