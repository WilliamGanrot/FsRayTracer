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

    let rings a b =
        { a = a; b = b; patternType = Rings; transform = Matrix.identityMatrix 4 }

    let checkers a b =
        { a = a; b = b; patternType = Checkers; transform = Matrix.identityMatrix 4 }

    let transform transformation (pattern:Pattern) =
        let transform = pattern.transform |> Transformation.applyToMatrix transformation
        { pattern with transform = transform }

    let at (point:Point) (pattern:Pattern) =

        match pattern.patternType with
        | Stripes  ->
            if (Math.Floor(point.X) |> int) % 2 = 0 then pattern.a else pattern.b
        | Gradient ->
            let distance = pattern.b - pattern.a
            let fraction = point.X - Math.Floor(point.X)
            pattern.a + Color.mulitplyByScalar fraction distance
        | Rings ->
            let distance = Math.Pow((point.X * point.X) + (point.Z * point.Z), 0.5)
            let disc = (Math.Floor(distance) |> int) % 2
            if (disc = 0) then pattern.a else pattern.b
        | Checkers ->
            let s = (Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) |> int
            if (s % 2 = 0) then pattern.a else pattern.b


