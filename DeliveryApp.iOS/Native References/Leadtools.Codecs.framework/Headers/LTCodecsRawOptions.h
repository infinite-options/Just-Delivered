//
//  LTCodecsRawOptions.h
//  Leadtools.Codecs
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsRawSaveOptions : NSObject

@property (nonatomic, assign) BOOL reverseBits;
@property (nonatomic, assign) BOOL pad4;

- (instancetype)init __unavailable;

@end

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsRawOptions : NSObject

@property (nonatomic, strong, readonly) LTCodecsRawSaveOptions *save;

- (instancetype)init __unavailable;

@end

NS_ASSUME_NONNULL_END
