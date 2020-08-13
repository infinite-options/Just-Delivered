//
//  LTCodecsVffOptions.h
//  Leadtools.Codecs
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

typedef NS_ENUM(NSInteger, LTCodecsVffView) {
    LTCodecsVffViewUpToDown,
    LTCodecsVffViewDownToUp,
    LTCodecsVffViewLeftToRight,
    LTCodecsVffViewRightToLeft,
    LTCodecsVffViewFrontToRear,
    LTCodecsVffViewRearToFront
};

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsVffLoadOptions : NSObject

@property (nonatomic, assign) LTCodecsVffView view;

- (instancetype)init __unavailable;

@end



NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsVffOptions : NSObject

@property (nonatomic, strong, readonly) LTCodecsVffLoadOptions *load;

- (instancetype)init __unavailable;

@end

NS_ASSUME_NONNULL_END
