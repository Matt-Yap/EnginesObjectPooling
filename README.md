# EnginesObjectPooling

Leux Mattheus Yap 100928496
Cube spawner
"gameplay loop" is to basically just press space to spawn a lot of cubes and "s" to stop spawning them
<img width="561" height="359" alt="image" src="https://github.com/user-attachments/assets/bb31d4c2-10a8-4665-891b-331e9f1ef818" />
the image on the bottom is before implementing the design pattern and the top is after

constant Instantiate and Destroy calls allocated memory almost every frame which caused the spikes
implemented Factory Method which replaced instantiate and destroy
GC allocated in frame had big spikes
the pool helped lessen all per-frame GC allocation and the large frame spikes, though frame time stayed similar, the gain is in smoothness and memory rather than the average speed
