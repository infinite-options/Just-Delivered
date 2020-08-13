//
//  LTCodecsEpsOptions.h
//  Leadtools.Codecs
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsEpsLoadOptions : NSObject

@property (nonatomic, assign) BOOL forceThumbnail;

- (instancetype)init __unavailable;

@end



NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsEpsOptions : NSObject

@property (nonatomic, strong, readonly) LTCodecsEpsLoadOptions *load;

- (instancetype)init __unavailable;

@end

NS_ASSUME_NONNULL_END
