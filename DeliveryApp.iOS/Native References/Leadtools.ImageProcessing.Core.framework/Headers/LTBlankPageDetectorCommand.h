//
//  LTBlankPageDetectorCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

typedef NS_OPTIONS(NSUInteger, LTBlankPageDetectorCommandFlags) {
    LTBlankPageDetectorCommandFlagsDetectEmptyPage        = 0x00000000,
    LTBlankPageDetectorCommandFlagsDetectNoisyPage        = 0x00000001,
    LTBlankPageDetectorCommandFlagsDontIgnoreBleedThrough = 0x00000000,
    LTBlankPageDetectorCommandFlagsIgnoreBleedThrough     = 0x00000010,
    LTBlankPageDetectorCommandFlagsDontDetectLinedPage    = 0x00000000,
    LTBlankPageDetectorCommandFlagsDetectLinedPage        = 0x00000100,
    LTBlankPageDetectorCommandFlagsDontUseActiveArea      = 0x00000000,
    LTBlankPageDetectorCommandFlagsUseActiveArea          = 0x00001000,
    LTBlankPageDetectorCommandFlagsUseDefaultMargins      = 0x00000000,
    LTBlankPageDetectorCommandFlagsUseUserMargins         = 0x00010000
};

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTBlankPageDetectorCommand : LTRasterCommand

@property (nonatomic, assign, readonly) BOOL isBlank;
@property (nonatomic, assign, readonly) NSUInteger accuracy;
@property (nonatomic, assign)           NSUInteger leftMargin;
@property (nonatomic, assign)           NSUInteger topMargin;
@property (nonatomic, assign)           NSUInteger rightMargin;
@property (nonatomic, assign)           NSUInteger bottomMargin;
@property (nonatomic, assign)           LTBlankPageDetectorCommandFlags flags;

- (instancetype)initWithFlags:(LTBlankPageDetectorCommandFlags)flags leftMargin:(NSUInteger)leftMargin topMargin:(NSUInteger)topMargin rightMargin:(NSUInteger)rightMargin bottomMargin:(NSUInteger)bottomMargin NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
