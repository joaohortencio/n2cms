#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections.Generic;
using System.Data;

namespace N2.Persistence
{
    /// <summary>
    /// The repository is a single point for database operations. All 
    /// persistence operations on database should pass through here.
    /// </summary>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
	public interface IRepository<TKey, TEntity> : IDisposable
	{
		/// <summary>
		/// Get the entity from the persistance store, or return null
		/// if it doesn't exist.
		/// </summary>
		/// <param name="id">The entity's id</param>
		/// <returns>Either the entity that matches the id, or a null</returns>
		TEntity Get(TKey id);

    	/// <summary>
    	/// Get the entity from the persistance store, or return null
    	/// if it doesn't exist.
    	/// </summary>
    	/// <param name="id">The entity's id</param>
    	/// <typeparam name="T">The type of entity to get.</typeparam>
    	/// <returns>Either the entity that matches the id, or a null</returns>
    	T Get<T>(TKey id);
		
		/// <summary>
		/// Load the entity from the persistance store
		/// Will throw an exception if there isn't an entity that matches
		/// the id.
		/// </summary>
		/// <param name="id">The entity's id</param>
		/// <returns>The entity that matches the id</returns>
		TEntity Load(TKey id);

		/// <summary>
		/// Register the entity for deletion when the unit of work
		/// is completed. 
		/// </summary>
		/// <param name="entity">The entity to delete</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Register te entity for save in the database when the unit of work
		/// is completed. (INSERT)
		/// </summary>
		/// <param name="entity">the entity to save</param>
		void Save(TEntity entity);

		/// <summary>
		/// Register the entity for update in the database when the unit of work
		/// is completed. (UPDATE)
		/// </summary>
		/// <param name="entity"></param>
		void Update(TEntity entity);

		/// <summary>
		/// Register te entity for save or update in the database when the unit of work
		/// is completed. (INSERT or UPDATE)
		/// </summary>
		/// <param name="entity">the entity to save</param>
		void SaveOrUpdate(TEntity entity);

		/// <summary>
		/// Check if any instance of the type exists
		/// </summary>
		/// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
		bool Exists();

		/// <summary>
		/// Counts the overall number of instances.
		/// </summary>
		/// <returns></returns>
		long Count();

		void Flush();

		ITransaction BeginTransaction();
	}
}
