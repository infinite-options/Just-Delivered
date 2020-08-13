//
//  LTBarcodeEngine.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTGlobalEnums.h>
#import <Leadtools.Barcode/LTBarcodeReader.h>
#import <Leadtools.Barcode/LTBarcodeWriter.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTBarcodeEngine : NSObject

@property (nonatomic, strong, readonly)        LTBarcodeReader *reader;
@property (nonatomic, strong, readonly)        LTBarcodeWriter *writer;

@property (class, nonatomic, strong, readonly) NSArray<NSNumber *> *supportedSymbologies;

+ (NSString *)friendlyNameForSymbology:(LTBarcodeSymbology)symbology;

@end

@interface LTBarcodeEngine (Deprecated)

+ (void)getSupportedSymbologies:(LTBarcodeSymbology * _Nullable * _Nonnull)supportedSymbologies supportedSymbologiesCount:(NSUInteger *)supportedSymbologiesCount LT_DEPRECATED_USENEW(19_0, "+[LTBarcodeEngine supportedSymboligies]");;
+ (void)freeSupportedSymbologies:(LTBarcodeSymbology *)supportedSymbologies LT_DEPRECATED(19_0);

+ (NSString *)getSymbologyFriendlyName:(LTBarcodeSymbology)symbology LT_DEPRECATED_USENEW(19_0, "friendlyNameForSymbology:");

@end

NS_ASSUME_NONNULL_END
