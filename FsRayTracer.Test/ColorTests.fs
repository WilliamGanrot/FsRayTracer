module ColorTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats
open RayTracer.Color
open RayTracer.ObjectDomain

open Xunit
open RayTracer.Helpers

[<Fact>]
let ``colors are (red,green,blue)`` () =

    let c = {Red = -0.5; Green = 0.4; Blue = 1.7}
    Assert.True(FloatHelper.equal c.Red -0.5)
    Assert.True(FloatHelper.equal c.Green 0.4)
    Assert.True(FloatHelper.equal c.Blue 1.7)
    0

[<Fact>]
let ``adding colors`` () =
    let c1 = {Red = 0.9; Green = 0.6; Blue = 0.75}
    let c2 = {Red = 0.7; Green = 0.1; Blue = 0.25}

    c1 + c2 .= {Red = 1.6; Green = 0.7; Blue = 1.0} |> Assert.True
    0

[<Fact>]
let ``subtracting colors`` () =
    let c1 = {Red = 0.9; Green = 0.6; Blue = 0.75}
    let c2 = {Red = 0.7; Green = 0.1; Blue = 0.25}

    let r = c1 - c2
    r .= {Red = 0.2; Green = 0.5; Blue = 0.5} |> Assert.True
    0

[<Fact>]
let ``multiplying a color by scalar`` () =
    let c = {Red = 0.2; Green = 0.3; Blue = 0.4}
    c * 2 .= {Red = 0.4; Green = 0.6; Blue = 0.8} |> Assert.True
    0

[<Fact>]
let ``multiplying colors`` () =
    let c1 = {Red = 1.0; Green = 0.2; Blue = 0.4}
    let c2 = {Red = 0.9; Green = 1.0; Blue = 0.1}
    c1 * c2 .= {Red = 0.9; Green = 0.2; Blue = 0.04} |> Assert.True
    0
