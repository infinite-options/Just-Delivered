//
//  LTFourStateBarcodeWriteOptions.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeWriteOptions.h>
#import <Leadtools.Barcode/LTBarcodeWriteEnums.h>
#import <Leadtools.Barcode/LTFourStateBarcodeEnums.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTFourStateBarcodeWriteOptions : LTBarcodeWriteOptions

@property (nonatomic, copy, readonly)  NSString *friendlyName;

@property (nonatomic, assign)          LTBarcodeOutputTextPosition textPosition;

@property (nonatomic, assign)          NSInteger xModule;

@property (nonatomic, assign)          LTAustralianPost4StateBarcodeCIFEncoding australianPostCIFEncoding;

@end

NS_ASSUME_NONNULL_END
