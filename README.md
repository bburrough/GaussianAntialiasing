Gaussian anti-aliasing improves image quality by randomly sampling the geometry within a pixel rather than sampling the geometry at the pixel center. The idea is brought over from ray-tracing where the color of a pixel is determined by randomly sampling geometry within the pixel many times and averaging the result.

Under typical rasterization procedures, the color of a pixel is determined by checking whether a triangle overlaps the pixel at its exact center point. This leads to a grating pattern commonly known as jaggies. Instead of sampling geometry at exact pixel centers, Gaussian anti-aliasing samples a random point within each pixel. As this occurs repeatedly as is the case realtime 3D content, the perceived color of the pixel converges towards a more accurate depiction of the geometry that lies within that pixel. 

With high frequency displays and realtime rendered content, the user just sees a smooth image free of jaggies.
