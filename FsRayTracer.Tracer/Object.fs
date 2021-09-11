namespace RayTracer.Object

open System
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Material
open RayTracer.Pattern
open RayTracer.Color
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Constnats
open RayTracer.ObjectDomain
open RayTracer.RenderingDomain


module Object =

    let transform t (object:Object) : Object =
        let t = Transformation.applyToMatrix t object.transform
        { object with transform = t; transformInverse = t |> Matrix.inverse }

    let normal point (object:Object) =

        let objectPoint =
            object.transformInverse
            |> Matrix.multiplyPoint point

        let objectNormal = object.localNormalAt object.shape objectPoint

        let worldNormal =
            object.transformInverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector objectNormal
        
        {worldNormal with W = 0.} |> Vector.normalize

    let setMaterial m object =
        {object with material = m}

    let pattern (pattern:Pattern) (object:Object) worldPoint =
        let objectPoint =
            object.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint worldPoint

        let patternPoint =
            pattern.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint objectPoint

        Pattern.at patternPoint pattern

    let lighting material light point eyevector normalv inShadow (object:Object) =

        let color =
            match material.pattern with
            | Some p        -> pattern p object point 
            | None          -> material.color
        
        let effectiveColor = color * light.intensity
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


