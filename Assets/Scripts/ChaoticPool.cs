using System;
using System.Collections.Generic;

namespace Asteroids3D  {
	public class ChaoticPool<T> where T : UnityEngine.Object {

		public delegate T Initializer();

		public int PoolSize { get; private set; }

		public int FreeObjectsCount => PoolSize - splitPointer;
		public int InuseObjectsCount => splitPointer;

		/// <summary>
		/// Points to first free element
		/// </summary>
		private int splitPointer = 0;

		private List<T> pool;

		/// <summary>
		/// Create and fill pool with 'empty' objects
		/// </summary>
		/// <param name="poolSize">Max object in pool</param>
		/// <param name="objectInitializer">Function that creates 'empty' object</param>
		public ChaoticPool(int poolSize, Initializer objectInitializer) {
			PoolSize = poolSize;
			pool = new List<T>(poolSize);
			for (int i = 0; i < poolSize; i++) {
				pool.Add(objectInitializer());
			}
		}

		/// <summary>
		/// Returns free object and marks it as inuse.
		/// If each object is inuse then throws NullReferenceException
		/// </summary>
		/// <returns>Free object</returns>
		/// <exception cref="NullReferenceException">When there is no free objects</exception>
		public T Get() {
			if (splitPointer >= PoolSize) {
				throw new NullReferenceException();
			}

			return pool[splitPointer++];
		}

		/// <summary>
		/// Returns object to pool
		/// <b>Important</b>: object must comes from this pool.
		/// </summary>
		/// <param name="obj">Object to return to pool</param>
		/// <exception cref="ArgumentOutOfRangeException">When pool contains no inuse objects</exception>
		/// <exception cref="ArgumentException">Returned object is free or not from this pool</exception>
		public void Return(T obj) {
			if(splitPointer<=0)
				throw new ArgumentOutOfRangeException(nameof(obj), 
					"This pool have no inuse objects. " +
					"Returned object must be already free or is from other pool.");
			
			if (InstanceIDEquals(pool[splitPointer - 1], obj)) {
				--splitPointer;
			}
			else {
				int curr = pool.IndexOf(obj);
				
				if(curr == -1)
					throw new ArgumentException("Returned object is not from this pool", nameof(obj));
				if(curr >= splitPointer)
					throw new ArgumentException("Returned object is already free", nameof(obj));
				
				//swap
				pool[curr] = pool[splitPointer - 1];
				pool[splitPointer - 1] = obj;
			}
		}



		private static bool InstanceIDEquals(UnityEngine.Object o1, UnityEngine.Object o2) {
			return o1.GetInstanceID() == o2.GetInstanceID();
		}
	}
}