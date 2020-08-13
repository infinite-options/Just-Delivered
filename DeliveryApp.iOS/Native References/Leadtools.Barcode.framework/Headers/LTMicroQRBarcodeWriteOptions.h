//
//  LTMicroQRBarcodeWriteOptions.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeWriteOptions.h>
#import <Leadtools.Barcode/LTMicroQRBarcodeEnums.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTMicroQRBarcodeWriteOptions : LTBarcodeWriteOptions

@property (nonatomic, copy, readonly)  NSString *friendlyName;

@property (nonatomic, assign)          NSInteger xModule;

@property (nonatomic, assign)          LTMicroQRBarcodeSymbolModel symbolModel;

@end

NS_ASSUME_NONNULL_END
