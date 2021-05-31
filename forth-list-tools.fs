\ forth list tools by gustav melck, sep 2020
\ vim: fdm=marker

private{  \ {{{

: gthrow  ( ior addr u -- )
    2 pick  if  type ." ; forth-list-tools error " dup . cr throw  else  2drop drop  then  ;

: (cons)  ( -- addr )  2 cells allocate s" cons error1" gthrow  ;
: (car!)  ( item addr -- )  postpone !  ;  immediate
: (cdr!)  ( item addr -- )  postpone cell+  postpone !  ;  immediate
: (car@)  ( item addr -- )  postpone @  ;  immediate
: (cdr@)  ( addr -- item )  postpone cell+  postpone @  ;  immediate

: (free-list)  ( free-item-xt list-addr in-loop? -- )
    if  r> drop  then
    ?dup 0=  if  drop  else
        >r dup  if  dup r@ (car@) swap execute  then
        r> dup (cdr@) swap free s" (free-list) error1" gthrow  true recurse
    then  ;

0 value q-head
0 value print-item-xt

: (print-list)  ( list in-loop? -- )
    if  r> drop  then
    ?dup 0<>  if
        >r  print-item-xt 0<>  if  r@ (car@) print-item-xt execute  else  r@ (car@) . cr  then
        r> (cdr@) true recurse
    then  ;

: (for-each-list-item)  ( xt list in-loop? -- )
    if  r> drop  then
    ?dup 0=  if  drop  else  2dup (car@) swap execute  (cdr@) true recurse  then  ;

: (list-length)  ( list count -- count' )
    dup 0>  if  r> drop  then  over 0=  if  nip  else  1+ swap (cdr@) swap recurse  then  ;

: (reverse-list)  ( old-list new-list -- list' )
    dup 0>  if  r> drop  then  over 0=  if  nip  else
        over (car@) (cons) dup >r (car!) r@ (cdr!)  (cdr@) r> recurse
    then  ;

}private  \ }}}

: cons  ( -- addr )  (cons)  ;
: car!  ( item addr -- )  (car!)  ;
: cdr!  ( item addr -- )  (cdr!)  ;
: car@  ( addr -- item )  (car@)  ;
: cdr@  ( addr -- item )  (cdr@)  ;

: +list  ( item addr -- addr' )  cons dup >r cdr! r@ car! r>  ;
: list+  ( item addr -- addr' )  cons >r  ?dup  if  r@ swap cdr!  then  r@ car! 0 r@ cdr! r>  ;

: snip-list  ( prev-cons snip-cons -- )  \ snips the cons and frees it
    >r ?dup 0<>  if  r@ cdr@ swap cdr!  then  r> free s" list-snip error1" gthrow  ;

: free-list  ( free-item-xt list-addr -- )  false (free-list)  ;

: >q  ( item -- q: item )  q-head +list to q-head  ;
: q>  ( q: item -- item q: )
    q-head 0= s" q> underflow" gthrow  q-head car@  q-head dup cdr@ to q-head  free s" q> error1" gthrow  ;
: 2>q  ( item1 item2 -- q: item1 item2 )  swap >q >q  ;
: 2q>  ( q: item1 item2 -- item1 item2 q: )  q> q> swap  ;
: q@  ( q: item -- item q: item )  q-head 0= s" q@ underflow" gthrow  q-head car@  ;
: 2q@  ( q: item1 item2 -- item1 item2 q: item1 item2 )
    q-head 0= s" 2q@ underflow" gthrow  q-head dup cdr@ to q-head q@  swap to q-head q@  ;

: print-list  ( print-item-xt list -- )  swap to print-item-xt false (print-list)  ;

: for-each-list-item  ( xt list -- )  false (for-each-list-item)  ;

: list-length  ( list -- count )  0 (list-length)  ;

: reverse-list  ( list -- list' )  0 (reverse-list)  ;

privatize

