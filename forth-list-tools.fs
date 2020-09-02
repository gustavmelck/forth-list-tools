\ forth list tools by gustav melck, sep 2020
\ vim: fdm=marker

private{  \ {{{

: gthrow  ( ior addr u -- )
    2 pick  if  type ." ; forth-list-tools error " dup . cr throw  else  2drop drop  then  ;

: (car@)  ( item addr -- )  postpone @  ;  immediate
: (cdr@)  ( addr -- item )  postpone cell+  postpone @  ;  immediate

: (free-list)  ( free-item-xt list-addr in-loop? -- )
    if  r> drop  then
    ?dup 0=  if  drop  else
        >r dup  if  dup r@ (car@) swap execute  then
        r> dup (cdr@) swap free s" (free-list) error1" gthrow  true recurse
    then  ;

0 value q-head

}private  \ }}}

: cons  ( -- addr )  2 cells allocate s" cons error1" gthrow  ;
: car!  ( item addr -- )  !  ;
: cdr!  ( item addr -- )  cell+ !  ;
: car@  ( addr -- item )  (car@)  ;
: cdr@  ( addr -- item )  (cdr@)  ;

: +list  ( item addr -- addr' )  cons dup >r cdr! r@ car! r>  ;
: list+  ( item addr -- addr' )  cons >r  ?dup  if  r@ swap cdr@  then  r@ car! 0 r@ cdr! r>  ;

: free-list  ( free-item-xt list-addr -- )  false (free-list)  ;

: >q  ( item -- q: item )  q-head +list to q-head  ;
: q>  ( q: item -- item q: )
    q-head 0= s" q> underflow" gthrow  q-head car@  q-head dup cdr@ to q-head  free s" q> error1" gthrow  ;
: 2>q  ( item1 item2 -- q: item1 item2 )  swap >q >q  ;
: 2q>  ( q: item1 item2 -- item1 item2 q: )  q> q> swap  ;
: q@  ( q: item -- item q: item )  q-head 0= s" q@ underflow" gthrow  q-head car@  ;
: 2q@  ( q: item1 item2 -- item1 item2 q: item1 item2 )
    q-head 0= s" 2q@ underflow" gthrow  q-head dup cdr@ to q-head q@  swap to q-head q@  ;

privatize

