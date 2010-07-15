using System;

namespace KlopModel
{
   internal class DisposableHelper : IDisposable
   {
      #region Fields and Constants

      private readonly Action end;

      #endregion

      #region Constructors

      public DisposableHelper(Action end)
      {
         this.end = end;
      }


      public DisposableHelper(Action begin, Action end)
      {
         this.end = end;
         begin();
      }

      #endregion

      #region Public methods

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         end();
      }

      #endregion
   }
}