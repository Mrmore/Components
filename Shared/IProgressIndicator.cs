using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renren.Components.Tools
{

    // Summary:
    //     Defines a provider for progress updates.
    //
    // Type parameters:
    //   T:
    //     The type of progress update value.This type parameter is contravariant. That
    //     is, you can use either the type you specified or any type that is less derived.
    //     For more information about covariance and contravariance, see Covariance
    //     and Contravariance in Generics.
    public interface IProgressIndicator<in T>
    {
        // Summary:
        //     Reports a progress update.
        //
        // Parameters:
        //   sender:
        //     The original sender emit this report
        //   value:
        //     The value of the updated progress.
        //   description:
        //     The additional description for reporting
        void Report(object sender, T value, string description);
    }

}
