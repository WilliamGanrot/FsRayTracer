namespace RayTracer.Material

open RayTracer.Color
open RayTracer.Light
open RayTracer.Point
open RayTracer.Vector
open System

[<AutoOpen>]
module Domain =
    type Material =
        { color: Color;
          ambient:float;
          diffuse:float;
          specular: float;
          shininess: float; }

module Material =

    let create ambient diffuse specular shinines color =
        {ambient = ambient; diffuse = diffuse; specular = specular; shininess = shinines; color = color}

    let standard =
        { color = Color.create 1. 1. 1.;
          ambient = 0.1;
          diffuse = 0.9;
          specular = 0.9;
          shininess = 200. }

    let withAmbient a m =
        {m with ambient = a}

    let withDiffuse d m =
        {m with diffuse = d}

    let withSpecular s m =
        {m with specular = s}

    let withShininess s m =
        {m with shininess = s}

    let withColor c m =
        {m with color = c}

    let lighting material light point eyevector normalv inShadow =

        let effectiveColor = material.color * light.intensity
        let lightv = (light.poistion - point) |> Vector.normalize
        let ambient = effectiveColor |> Color.mulitplyByScalar material.ambient
        let lightDotNormal = Vector.dot lightv normalv

        let diffuse =
            effectiveColor
            |> Color.mulitplyByScalar (material.diffuse * lightDotNormal)

        let reflectDotEye =
            Vector.reflect normalv (lightv * -1.)
            |> Vector.dot eyevector

        match inShadow with
        | true -> ambient
        | false ->
            match lightDotNormal with
            | v when v < 0. -> ambient
            | _ when reflectDotEye <= 0. -> ambient + diffuse
            | _ ->
                let factor = Math.Pow(reflectDotEye, material.shininess)
                ambient + diffuse + (light.intensity |> Color.mulitplyByScalar (material.specular * factor))