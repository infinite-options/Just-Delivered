//
//  LTInvertedPageCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

typedef NS_OPTIONS(NSUInteger, LTInvertedPageCommandFlags) {
    LTInvertedPageCommandFlagsNone      = 0x0000,
    LTInvertedPageCommandFlagsProcess   = 0x00000000,
    LTInvertedPageCommandFlagsNoProcess = 0x00000001
};

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTInvertedPageCommand : LTRasterCommand

@property (nonatomic, assign, readonly) BOOL isInverted;
@property (nonatomic, assign)           LTInvertedPageCommandFlags flags;

- (instancetype)initWithFlags:(LTInvertedPageCommandFlags)flags NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
