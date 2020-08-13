//
//  LTDatamatrixBarcodeReadOptions.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeReadOptions.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTDatamatrixBarcodeReadOptions : LTBarcodeReadOptions

@property (nonatomic, copy, readonly)  NSString *friendlyName;

@property (nonatomic, assign)          BOOL enableDoublePass;
@property (nonatomic, assign)          BOOL enableDoublePassIfSuccess;
@property (nonatomic, assign)          BOOL enableInvertedSymbols;
@property (nonatomic, assign)          BOOL enableFastMode;
@property (nonatomic, assign)          BOOL readSquareSymbolsOnly;
@property (nonatomic, assign)          BOOL enableSmallSymbols;
@property (nonatomic, assign)          BOOL enablePreprocessing;
@property (nonatomic, assign)          BOOL GS1toHRI;

@end

NS_ASSUME_NONNULL_END
