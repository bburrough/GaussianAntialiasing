# Gaussian Anti-aliasing

Gaussian anti-aliasing is a improves image quality by randomly sampling the geometry within a pixel rather than sampling the geometry at the pixel center. The idea is brought over from ray-tracing where the color of a pixel is determined by randomly sampling geometry within the pixel many times and averaging the result. It achieves perceptual smoothing of rendered images with zero impact to rendering performance.

![screenshot](https://user-images.githubusercontent.com/12551477/135394232-5b979530-fb7a-45f1-a09f-3419ad722a04.png)

\
Under traditional [rasterization](https://en.wikipedia.org/wiki/Rasterisation) procedures, the color of a pixel is determined by checking whether a triangle overlaps the pixel at its exact center point. This leads to a grating pattern commonly known as jaggies. Instead of sampling geometry at exact pixel centers, Gaussian anti-aliasing samples a random point within each pixel. As this occurs repeatedly as is the case with realtime 3D content, the perceived color of the pixel converges towards a more accurate depiction of the geometry that lies within that pixel. 

https://user-images.githubusercontent.com/12551477/138059517-0ef61833-b336-4361-8afb-06e902ff11f4.mp4

\
Due to [persistence of vision](https://en.wikipedia.org/wiki/Persistence_of_vision) — the effect that causes us to see motion in a sequence of animated frames — successive frames appear blended. With high frequency displays and realtime rendered content, the user just sees a smooth image free of jaggies. Whereas temporal anti-aliasing adds additional processing steps by jittering and blending a sequence of frames, Gaussian anti-aliasing achieves a smoother image with zero impact to performance.
  
![comparison](https://user-images.githubusercontent.com/12551477/135394069-955a984c-a4f9-404f-aad4-de9e0a2bc486.png)


### License

This project is made availble under either the MIT or public domain licenses. Choose whichever you prefer.
