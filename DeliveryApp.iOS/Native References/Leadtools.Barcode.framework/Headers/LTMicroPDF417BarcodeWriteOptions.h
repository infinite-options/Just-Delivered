//
//  LTMicroPDF417BarcodeWriteOptions.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeWriteOptions.h>
#import <Leadtools.Barcode/LTBarcodeWriteEnums.h>
#import <Leadtools.Barcode/LTMicroPDF417BarcodeEnums.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTMicroPDF417BarcodeWriteOptions : LTBarcodeWriteOptions

@property (nonatomic, copy, readonly)  NSString *friendlyName;

@property (nonatomic, assign)          LTBarcodeAlignment horizontalAlignment;
@property (nonatomic, assign)          LTBarcodeAlignment verticalAlignment;

@property (nonatomic, assign)          LTMicroPDF417BarcodeSymbolSize symbolSize;

@property (nonatomic, assign)          BOOL useMode128Emulation;
@property (nonatomic, assign)          BOOL isLinked;
@property (nonatomic, assign)          BOOL enableGroupMode;
@property (nonatomic, assign)          BOOL limitByRowsAndColumns;

@property (nonatomic, assign)          NSInteger xModule;
@property (nonatomic, assign)          NSInteger xModuleAspectRatio;

@end

NS_ASSUME_NONNULL_END
