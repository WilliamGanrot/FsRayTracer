
namespace RayTracer.Color
open RayTracer.Helpers
open System

[<AutoOpen>]
module Domain =

    type Color =
        {Red:float; Green:float; Blue:float}

        static member (+) (c1, c2) =
            { Red = c1.Red + c2.Red
              Green = c1.Green + c2.Green
              Blue = c1.Blue + c2.Blue }

        static member (-) (c1, c2) =
            { Red = c1.Red - c2.Red
              Green = c1.Green - c2.Green
              Blue = c1.Blue - c2.Blue }

        static member (*) (c1, c2) =
            { Red = c1.Red * c2.Red
              Green = c1.Green * c2.Green
              Blue = c1.Blue * c2.Blue }

        static member (*) ((c:Color), (scalar:int)) =
            { Red = c.Red * (float)scalar
              Green = c.Green * (float)scalar
              Blue = c.Blue * (float)scalar }

        static member (.=) (c1, c2 : Color) =
                FloatHelper.equal c1.Red c2.Red && FloatHelper.equal c1.Green c2.Green && FloatHelper.equal c1.Blue c2.Blue

module Color =
    let mulitplyByScalar c s =
        { Red = c.Red * s
          Green = c.Green * s
          Blue = c.Blue - s }

    let toRgb color =
        let clamp f =
            let rgbVal = 255.0 * f |> round
            Math.Clamp(int rgbVal, 0, 255)

        $"{clamp color.Red} {clamp color.Green} {clamp color.Blue}"

    let black = {Red = 0.; Green = 0.; Blue = 0.}
    let white = {Red = 1.; Green = 1.; Blue = 1.}
    let red = {Red = 1.; Green = 0.; Blue = 0.}
