namespace RayTracer.Pattern

open RayTracer.Color
open RayTracer.Point
open RayTracer.Matrix
open RayTracer.Transformation

open System

[<AutoOpen>]
module Domain =

    type PatternType =
        | Stripes
        | Gradient
        | Rings
        | Checkers

    type Pattern = { transform: Matrix; patternType: PatternType; a: Color; b:Color; }

module Pattern =

    let stripes a b =
        { a = a; b = b; patternType = Stripes; transform = Matrix.identityMatrix 4 }

    let gradient a b =
        { a = a; b = b; patternType = Gradient; transform = Matrix.identityMatrix 4 }

    let at (point:Point) (pattern:Pattern) =
        match pattern.patternType with
        | Stripes  ->
            if (Math.Floor(point.X) |> int) % 2 = 0 then pattern.a else pattern.b
        | Gradient ->
            let distance = pattern.b - pattern.a
            let fraction = point.X - Math.Floor(point.X)
            pattern.a + Color.mulitplyByScalar fraction distance

    let transform transformation (pattern:Pattern) =
        let transform = pattern.transform |> Transformation.applyToMatrix transformation
        { pattern with transform = transform }
