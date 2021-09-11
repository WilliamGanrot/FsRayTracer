
namespace RayTracer.Color
open RayTracer.Helpers
open RayTracer.ObjectDomain
open System


module Color =

    let create r g b =
        { Red = r; Green = g; Blue = b }

    let mulitplyByScalar s c =
        { Red = c.Red * s
          Green = c.Green * s
          Blue = c.Blue * s }

    let toRgb color =
        let clamp f =
            let rgbVal = 255.0 * f |> round
            Math.Clamp(int rgbVal, 0, 255)

        $"{clamp color.Red} {clamp color.Green} {clamp color.Blue}"

    let black = {Red = 0.; Green = 0.; Blue = 0.}
    let white = {Red = 1.; Green = 1.; Blue = 1.}
    let red = {Red = 1.; Green = 0.; Blue = 0.}
