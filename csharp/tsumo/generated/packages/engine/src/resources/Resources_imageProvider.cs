using System;

namespace Tsumo.Engine
{
    public static class Resources_imageProvider
    {
        public static bool imageCodecsRegistered
        {
            get;
            internal set;
        } = default(bool)!;
        public static Action ensureImageCodecsRegistered
        {
            get;
            private set;
        } = default(Action)!;
        public static Func<string, string, int> parsePositiveDimension
        {
            get;
            private set;
        } = default(Func<string, string, int>)!;
        public static Func<string, ImageResizeRequest> parseImageResizeRequest
        {
            get;
            private set;
        } = default(Func<string, ImageResizeRequest>)!;
        public static Func<Resource, string, Resource> resizeImageResource
        {
            get;
            private set;
        } = default(Func<Resource, string, Resource>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Resources_imageDimensions.__tsonic_module_init();
            Resources_mediaTypes.__tsonic_module_init();
            Resources_models.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            imageCodecsRegistered = false;
            ensureImageCodecsRegistered = () =>
            {
                if (imageCodecsRegistered)
                {
                    return;
                }
                PhotoSauce.MagicScaler.CodecManager.Configure((PhotoSauce.MagicScaler.CodecCollection codecs) =>
                {
                    PhotoSauce.NativeCodecs.Libpng.CodecCollectionExtensions.UseLibpng(codecs, true);
                    PhotoSauce.NativeCodecs.Libjpeg.CodecCollectionExtensions.UseLibjpeg(codecs, true);
                    PhotoSauce.NativeCodecs.Giflib.CodecCollectionExtensions.UseGiflib(codecs, true);
                    PhotoSauce.NativeCodecs.Libwebp.WebpCodec.UseLibwebp(codecs, true);
                });
                imageCodecsRegistered = true;
            };
            parsePositiveDimension = (string value, string spec) =>
            {
                if (value == "")
                {
                    return 0;
                }
                int? parsed = Utils_int32.parseInt32(value);
                if (parsed is null || parsed.Value <= 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_IMAGE_RESIZE_SPEC_INVALID", $"Invalid image resize specification: {spec}");
                }
                return parsed.Value;
            };
            parseImageResizeRequest = (string spec) =>
            {
                Tsonic.CSharp.Js.JSArray<string> tokens = Tsonic.CSharp.Js.String.split(Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(spec)), " ").filter((string token, int _, Tsonic.CSharp.Js.JSArray<string> _) => token != "");
                if (tokens.length == 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_IMAGE_RESIZE_SPEC_INVALID", "Image resize specification cannot be empty");
                }
                string dimensions = tokens[0];
                int separator = Tsonic.CSharp.Js.String.indexOf(dimensions, "x");
                int width = default(int)!;
                int height = default(int)!;
                if (separator < 0)
                {
                    width = parsePositiveDimension(dimensions, spec);
                    height = 0;
                }
                else
                {
                    width = parsePositiveDimension(Tsonic.CSharp.Js.String.slice(dimensions, 0, separator), spec);
                    height = parsePositiveDimension(Tsonic.CSharp.Js.String.slice(dimensions, separator + 1), spec);
                }
                if (width == 0 && height == 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_IMAGE_RESIZE_SPEC_INVALID", $"Invalid image resize specification: {spec}");
                }
                string? format = null;
                for (int index = 1; index < tokens.length; index++)
                {
                    string token = tokens[index];
                    if (token == "jpg" || token == "jpeg" || token == "png" || token == "gif" || token == "webp")
                    {
                        if (format is not null)
                        {
                            throw Diagnostics.createTsumoError("TSUMO_IMAGE_RESIZE_SPEC_INVALID", $"Image resize format is specified more than once: {spec}");
                        }
                        format = token == "jpeg" ? "jpg" : token;
                        continue;
                    }
                    throw Diagnostics.createTsumoError("TSUMO_IMAGE_RESIZE_OPTION_UNSUPPORTED", $"Unsupported image resize option '{token}'");
                }
                return new ImageResizeRequest(width, height, format);
            };
            resizeImageResource = (Resource resource, string specification) =>
            {
                ImageResizeRequest request = parseImageResizeRequest(specification);
                int width = request.width;
                int height = request.height;
                if (width == 0 && resource.width > 0 && resource.height > 0)
                {
                    width = (resource.width * height) / resource.height;
                }
                else
                {
                    if (height == 0 && resource.width > 0 && resource.height > 0)
                    {
                        height = (resource.height * width) / resource.width;
                    }
                }
                if (width <= 0 || height <= 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_IMAGE_DIMENSIONS_UNKNOWN", "Image resizing with one automatic dimension requires known source dimensions");
                }
                string sourceName = resource.outputRelPath ?? resource.sourcePath ?? "";
                string sourceExtension = Tsonic.CSharp.Js.String.toLowerCase((System.IO.Path.GetExtension(sourceName) ?? ""));
                if (sourceExtension == "")
                {
                    throw Diagnostics.createTsumoError("TSUMO_IMAGE_FORMAT_UNKNOWN", "Image resizing requires a source file format");
                }
                string outputExtension = request.format is null ? sourceExtension : $".{request.format}";
                string workDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"tsumo-image-{System.Guid.NewGuid().ToString("n")}");
                System.IO.Directory.CreateDirectory(workDirectory);
                try
                {
                    string inputPath = System.IO.Path.Combine(workDirectory, "input" + sourceExtension);
                    string outputPath = System.IO.Path.Combine(workDirectory, "output" + outputExtension);
                    Tsonic.CSharp.Node.fs.writeFileSync(inputPath, resource.bytes);
                    ensureImageCodecsRegistered();
                    PhotoSauce.MagicScaler.ProcessImageSettings settings = new PhotoSauce.MagicScaler.ProcessImageSettings();
                    settings.Width = width;
                    settings.Height = height;
                    if (request.format is not null)
                    {
                        settings.TrySetEncoderFormat(outputExtension);
                    }
                    PhotoSauce.MagicScaler.MagicImageProcessor.ProcessImage(inputPath, outputPath, settings);
                    Tsonic.CSharp.Node.Buffer outputBytes = Tsonic.CSharp.Node.fs.readFileSync(outputPath);
                    int outputWidth = width;
                    int outputHeight = height;
                    ImageDimensions? dimensions = Resources_imageDimensions.parseImageDimensions(outputBytes);
                    if (dimensions is not null)
                    {
                        outputWidth = dimensions.width;
                        outputHeight = dimensions.height;
                    }
                    string outputRelPath = resource.outputRelPath ?? "";
                    ResourcePathParts path = Resources_paths.splitResourcePath(outputRelPath);
                    ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                    string outputFile = $"{file.baseName}_{outputWidth}x{outputHeight}{outputExtension}";
                    return new Resource($"{resource.id}|resize:{specification}", null, true, path.directory + outputFile, outputBytes, null, new ResourceData(""), Resources_mediaTypes.resourceMediaTypeForExtension(outputExtension), outputWidth, outputHeight);
                }
                finally
                {
                    if (System.IO.Directory.Exists(workDirectory))
                    {
                        System.IO.Directory.Delete(workDirectory, true);
                    }
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ImageResizeRequest
    {
        public int width;
        public int height;
        public string? format;
        public ImageResizeRequest(int width, int height, string? format)
        {
            this.width = width;
            this.height = height;
            this.format = format;
        }
    }
}
