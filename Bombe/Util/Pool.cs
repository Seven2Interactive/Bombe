// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Bombe {

	/// <summary>
	/// A pool of reusable objects that can be used to avoid allocation.
	/// Create a pool and preallocate it with 10 ExpensiveObjects:
	/// Pool<MyObj> objectPool = new Pool<MyObj>((newObj MyObj) => return new MyObj()).SetSize(10);
	/// 
	/// MyObj obj = objectPool.Take();
	/// .... Do stuff, then put it back into the pool.
	/// objectPool.Put(obj);
	/// 
	/// Optionally add a Deallocator to reset your object to it's starting state, so it's ready to
	/// go when you call Pool.Take(). Deallocator is also called if you call Pool.Dispose() ...
	/// 
	/// objectPool.SetDeallocator((oldObj) => {
	/// 	oldObj.x = 0;
	/// 	oldObj.y = 0;
	/// 	oldObj.z = 0; // Whatever other cleanup you need to do.
	/// });
	/// 
	/// </summary>
	public class Pool<A> : Disposable
	{
		/// The allocator to use to generate new objects.
		private Func<A> _allocator;
		/// The Stack of objects ready to be used.
		private Stack<A> _freeObjects;
		private int _capacity = 2147483647;

		public Action<A> deallocator;

		/* ---------------------------------------------------------------------------------------- */
		      
		public Pool(Func<A> allocator)
		{
			_allocator = allocator;
			_freeObjects = new Stack<A>();
		}
		
		/* ---------------------------------------------------------------------------------------- */

		public Pool<A> SetDeallocator(Action<A> dealloc) {
			deallocator = dealloc;
			return this;
		}
		
		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Releases all resource used by the <see cref="Bombe.Pool`1"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Bombe.Pool`1"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="Bombe.Pool`1"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="Bombe.Pool`1"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Bombe.Pool`1"/> was occupying.</remarks>
		public void Dispose() {
			if (deallocator != null) {
				A[] objects = _freeObjects.ToArray();
				int ii = objects.Length;
				while (ii-->0) {
					deallocator(objects[ii]);
				}
			}
			_freeObjects.Clear();
		}
		      
		/* ---------------------------------------------------------------------------------------- */
		      
		///
		/// Take an object from the pool. If the pool is empty, a new object will be allocated.
		/// You should later release the object back into the pool by calling `put()`.
		///
		public A Take ()
		{
			if (_freeObjects.Count > 0) {
				return _freeObjects.Pop();
			}
			A item = _allocator();
#if DEBUG
			if (item == null) {
				Debug.LogError("An object in your pool is null!");
			}
#endif

			return item;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      		
		/// <summary>
		/// Put an object into the pool. This should be called to release objects previously claimed with
		/// `Take()`. Can also be called to pre-allocate the pool with new objects.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Put (A item)
		{
#if DEBUG
			if (item == null) {
				Debug.LogError("You are attempting to add a null object into your pool!");
			}
#endif
			if (_freeObjects.Count < _capacity) {
				_freeObjects.Push(item);
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      		
		/// <summary>
		/// Resizes the pool. If the given size is larger than the current number of pooled objects, new
		/// objects are allocated to expand the pool. Otherwise, objects are trimmed out of the pool.
		/// </summary>
		/// <returns>This instance, for chaining</returns>
		/// <param name="size">The size of the pool to prepopulate with.</param>
		public Pool<A> SetSize(int size)
		{
			if (_freeObjects.Count > size) {
				int ii = size;
				while (ii-->0) {
					_freeObjects.Pop();
				}
//				_freeObjects.Resize(size);
			} else {
				int needed = size - _freeObjects.Count;
				for (int ii = 0; ii < needed; ii++) {
					A item = _allocator();
#if DEBUG
					if (item == null) {
						Debug.LogError("Object cannot be null!");
					}
#endif
					_freeObjects.Push(item);
				}
			}
			return this;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		/// <summary>
		/// Sets the maximum capacity of the pool. By default, the pool can grow to any size.
		/// </summary>
		/// <returns>This pool for chaining purposes.</returns>
		/// <param name="capacity">Capacity.</param>
		public Pool<A> SetCapacity (int capacity)
		{
			if (_freeObjects.Count > capacity) {
//				_freeObjects.resize(capacity);
			}
			_capacity = capacity;
			return this;
		}
		
	}


}