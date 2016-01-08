using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkKnight.client.Crypt
{
    public abstract class AbstractCrypt
    {
        /// <summary>
        /// Encode a package with your own encrypts algorithm
        /// </summary>
        /// <param name="package">The packet to be encrypted</param>
        /// <returns>The packet encoded</returns>
        public abstract byte[] encode(byte[] package);

        /// <summary>
        /// Decode a package with you own decrypt algorithm
        /// </summary>
        /// <param name="package">The packet to be decoded</param>
        /// <returns>The packet decoded</returns>
        public abstract byte[] decode(byte[] package);
    }
}
