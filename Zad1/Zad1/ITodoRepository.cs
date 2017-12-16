
using System;
using System.Collections.Generic;

namespace Zad1
{
    public interface ITodoRepository
    {
        // <summary >
        /// Gets  TodoItem  for a given id. Throw  TodoAccessDeniedException with appropriate  message  if user is not the  owner of the Todo  item
        /// </summary >
        /// <param  name=" todoId">Todo Id </param >
        /// <param  name=" userId">Id of the  user  that is  trying  to  fetch  the data</param >
        /// <returns >TodoItem  if found , null  otherwise </returns >
        TodoItem Get(Guid todoId, Guid userId);
        
        /// <summary >
        /// Adds  new  TodoItem  object  in  database.
        /// If  object  with  the  same id  already  exists ,
        /// method  should  throw  DuplicateTodoItemException  with  the  message"duplicate  id: {id}".
        ///  </summary >
        void Add(TodoItem todoItem);
       
        /// <summary >
        /// Tries to  remove a TodoItem  with  given id from  the  database. Throw TodoAccessDeniedException with  appropriate message  if user is not the  owner of the Todo  item
        /// </summary >
        /// <param  name=" todoId">Todo Id </param >
        /// <param  name=" userId">Id of the  user  that is  trying  to  remove  the data</param >
        /// <returns >True if success , false  otherwise </returns >
        bool Remove(Guid todoId, Guid userId);
       
        /// <summary >
        /// Updates  given  TodoItem  in the  database.
        /// If  TodoItem  does  not exist , method  will  add  one.
        ///  </summary >
        void Update(TodoItem todoItem);
        
        /// <summary >
        /// Tries to mark a TodoItem  as  completed  in  database. Throw TodoAccessDeniedException with  appropriate message  if user is not the  owner of the Todo  item
        /// </summary >
        /// <param  name=" todoId">Todo Id </param >
        /// <param  name=" userId">Id of the  user  that is  trying  to mark as completed</param >
        /// <returns >True if success , false  otherwise </returns >
        bool MarkAsCompleted(Guid todoId, Guid userId);
        
        /// <summary >
        /// Gets  all  TodoItem  objects  in  database  for user , sorted  by date created(descending)
        /// </summary >
        List<TodoItem> GetAll(Guid userId);
        
        /// <summary >
        /// Gets  all  incomplete  TodoItem  objects  in  database  for  user
        /// </summary >
        List<TodoItem> GetActive(Guid userId);
       
        /// <summary >
        /// Gets  all  completed  TodoItem  objects  in  database  for  user
        /// </summary >
        List<TodoItem> GetCompleted(Guid userId);
        
        /// <summary >
        /// Gets  all  TodoItem  objects  in  database  for  user  that  apply to the filter
        /// </summary >
        List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId);


         bool ConatinsLabel(string value);

         void AddLabel(string value);


         void AddLabelToTodoItem(String value, Guid itemId, Guid userGuid);

    }
}