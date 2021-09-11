namespace RayTracer.Cube
open RayTracer.Constnats
open RayTracer.Vector
open RayTracer.Point

open System
open RayTracer.Intersection


module Cube =
    let checkAxis origin direction =
        let tMinNumerator = (-1.) - origin
        let tMaxNumerator = 1. - origin

        let tMin, tMax =
            match Math.Abs(direction:float) >= epsilon with
            | true ->
                let tMin = tMinNumerator / direction
                let tMax = tMaxNumerator / direction
                (tMin, tMax)
            | false ->
                let tMin = tMinNumerator * infinity
                let tMax = tMaxNumerator * infinity
                (tMin, tMax)

        if tMin > tMax then (tMax, tMin) else (tMin, tMax)


